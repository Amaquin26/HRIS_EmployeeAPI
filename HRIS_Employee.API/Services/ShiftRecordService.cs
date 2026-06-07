using HRIS_Employee.API.DTOs.Schedule;
using HRIS_Employee.API.Repositories.Base;
using HRIS_Employee.API.Services.UnitOfWork;
using HRIS_Employee.Infrastructure.Domain.Enums;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.API.Services
{
    public class ShiftRecordService(
        IBaseRepository<ShiftRecord> shiftRecordRepository,
        IBaseRepository<Employee> employeeRepository,
        IUnitOfWork unitOfWork) : IShiftRecordService
    {
        private const int UndoWindowMinutes = 5;

        public async Task<ShiftRecordDto> ClockInAsync(string entraObjectId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            // fetch employee with schedule for validation
            var employee = await employeeRepository.GetSingleAsync(
                x => x.EntraObjectId == entraObjectId,
                q => q.Include(x => x.Schedule)
                      .ThenInclude(x => x.ScheduleDays)
                      .Include(x => x.Schedule)
                      .ThenInclude(x => x.ScheduleOverrides));


            if (employee is null)
                throw new InvalidOperationException("Employee not found.");

            // block double clock-in
            var existingShift = await shiftRecordRepository.GetSingleAsync(
                x => x.EmployeeId == employee.Id && x.ShiftDate == today);

            if (existingShift is not null)
                throw new InvalidOperationException("Already clocked in for today."); // TODO: custom exception


            var now = DateTimeOffset.UtcNow;
            var (isFlagged, flagReason) = ValidateClockIn(employee.Schedule, today, now);

            var shiftRecord = new ShiftRecord
            {
                EmployeeId = employee.Id,
                ShiftDate = today,
                ClockIn = now,
                Status = isFlagged ? ShiftStatus.Flagged : ShiftStatus.Open,
                IsFlagged = isFlagged,
                FlagReason = flagReason,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await shiftRecordRepository.AddAsync(shiftRecord);
            await unitOfWork.SaveChangesAsync();

            return new ShiftRecordDto
            {
                Id = shiftRecord.Id,
                EmployeeId = shiftRecord.EmployeeId,
                ShiftDate = shiftRecord.ShiftDate,
                ClockIn = shiftRecord.ClockIn,
                ClockOut = shiftRecord.ClockOut,
                Status = shiftRecord.Status,
                IsFlagged = shiftRecord.IsFlagged,
                FlagReason = shiftRecord.FlagReason,
                CreatedAt = shiftRecord.CreatedAt
            };
        }

        public async Task<ShiftRecordDto> ClockOutAsync(string entraObjectId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var shiftRecord = await shiftRecordRepository.GetSingleAsync(
                x => x.Employee.EntraObjectId == entraObjectId && x.ShiftDate == today);

            if (shiftRecord is null)
                throw new InvalidOperationException("No open shift found for today.");

            if (shiftRecord.ClockOut is not null)
                throw new InvalidOperationException("Already clocked out for today.");

            shiftRecord.ClockOut = DateTimeOffset.UtcNow;
            shiftRecord.Status = shiftRecord.IsFlagged ? ShiftStatus.Flagged : ShiftStatus.Completed;

            shiftRecordRepository.UpdateAsync(shiftRecord);
            await unitOfWork.SaveChangesAsync();

            return new ShiftRecordDto
            {
                Id = shiftRecord.Id,
                EmployeeId = shiftRecord.EmployeeId,
                ShiftDate = shiftRecord.ShiftDate,
                ClockIn = shiftRecord.ClockIn,
                ClockOut = shiftRecord.ClockOut,
                Status = shiftRecord.Status,
                IsFlagged = shiftRecord.IsFlagged,
                FlagReason = shiftRecord.FlagReason,
                CreatedAt = shiftRecord.CreatedAt
            };
        }

        public async Task<ShiftRecordDto> UndoClockOutAsync(string entraObjectId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var shiftRecord = await shiftRecordRepository.GetSingleAsync(
                x => x.Employee.EntraObjectId == entraObjectId && x.ShiftDate == today);

            if (shiftRecord is null)
                throw new InvalidOperationException("No shift found for today.");

            if (shiftRecord.ClockOut is null)
                throw new InvalidOperationException("Not clocked out yet.");

            var minutesSinceClockOut = (DateTimeOffset.UtcNow - shiftRecord.ClockOut.Value).TotalMinutes;

            if (minutesSinceClockOut > UndoWindowMinutes)
                throw new InvalidOperationException($"Undo window of {UndoWindowMinutes} minutes has expired.");

            shiftRecord.ClockOut = null;
            shiftRecord.Status = shiftRecord.IsFlagged ? ShiftStatus.Flagged : ShiftStatus.Open;

            shiftRecordRepository.UpdateAsync(shiftRecord);
            await unitOfWork.SaveChangesAsync();

            return new ShiftRecordDto
            {
                Id = shiftRecord.Id,
                EmployeeId = shiftRecord.EmployeeId,
                ShiftDate = shiftRecord.ShiftDate,
                ClockIn = shiftRecord.ClockIn,
                ClockOut = shiftRecord.ClockOut,
                Status = shiftRecord.Status,
                IsFlagged = shiftRecord.IsFlagged,
                FlagReason = shiftRecord.FlagReason,
                CreatedAt = shiftRecord.CreatedAt
            };
        }

        public async Task<EmployeeScheduleDetailDto> GetEmployeeShiftDetailAsync(string entraObjectId)
        {
            var daysToCapture = 5;
            var today = DateOnly.FromDateTime(DateTime.Today);

            var employee = await employeeRepository.GetSingleAsync(
                x => x.EntraObjectId == entraObjectId,
                q => q.Include(x => x.Schedule)
                       .ThenInclude(x => x.ScheduleDays)
                       .Include(x => x.Schedule)
                       .ThenInclude(x => x.ScheduleOverrides));

            if (employee is null)
                throw new Exception("Employee not found."); // TODO: custom exception

            var schedule = employee.Schedule;

            // today's schedule
            var todaySchedule = ResolveScheduleDay(schedule, today);

            // fetch shift records for today and past 3 days
            var from = today.AddDays(-daysToCapture);
            var shiftRecords = await shiftRecordRepository.GetAllAsync(
                x => x.EmployeeId == employee.Id && x.ShiftDate >= from && x.ShiftDate <= today && x.ShiftDate >= DateOnly.FromDateTime(employee.HiredDate.DateTime));

            // build history — past 3 days only (exclude today)
            var recentShifts = new List<ShiftHistoryDto>();

            for (var i = 0; i < daysToCapture; i++)
            {
                var date = today.AddDays(-i);
                var resolved = ResolveScheduleDay(schedule, date);

                var record = shiftRecords.FirstOrDefault(x => x.ShiftDate == date);

                // skip rest days — no log expected
                if (resolved.IsRestDay && record?.ClockIn == null)
                    continue;

                if (record is not null)
                {
                    recentShifts.Add(new ShiftHistoryDto
                    {
                        Id = record.Id,
                        ShiftDate = record.ShiftDate,
                        ClockIn = record.ClockIn,
                        ClockOut = record.ClockOut,
                        Status = record.Status,
                        IsFlagged = record.IsFlagged,
                        FlagReason = record.FlagReason,
                        IsMissing = false
                    });
                }
                else
                {
                    // working day with no record = missing
                    recentShifts.Add(new ShiftHistoryDto
                    {
                        Id = null,
                        ShiftDate = date,
                        ClockIn = null,
                        ClockOut = null,
                        Status = null,
                        IsFlagged = false,
                        FlagReason = null,
                        IsMissing = true
                    });
                }
            }

            return new EmployeeScheduleDetailDto 
            {
                TodaySchedule = todaySchedule,
                RecentShifts = recentShifts.OrderByDescending(x => x.ShiftDate).ToList()
            };
        }

        private static (bool isFlagged, string? flagReason) ValidateClockIn(
            Infrastructure.Persistence.Models.Schedule schedule, DateOnly today, DateTimeOffset now)
        {
            // check override first, fall back to schedule day
            var override_ = schedule.ScheduleOverrides
                .FirstOrDefault(x => x.SpecificDate == today);

            if (override_ is not null)
            {
                if (override_.IsRestDay)
                    return (true, "Clocked in on a rest day (override).");
                return (false, null);
            }

            var scheduleDay = schedule.ScheduleDays
                .FirstOrDefault(x => x.DayOfWeek == today.DayOfWeek);

            if (scheduleDay is null || scheduleDay.IsRestDay)
                return (true, "Clocked in on a rest day.");

            return (false, null);
        }

        private static ScheduledTimeDto ResolveScheduleDay(Infrastructure.Persistence.Models.Schedule schedule, DateOnly date)
        {
            // check override first
            var override_ = schedule.ScheduleOverrides
                .FirstOrDefault(x => x.SpecificDate == date);

            if (override_ is not null)
            {
                return new ScheduledTimeDto
                {

                    StartTime = override_.StartTime.HasValue ? DateTime.Today.Add(override_.StartTime.Value.ToTimeSpan()) : null,
                    EndTime = override_.EndTime.HasValue ? DateTime.Today.Add(override_.EndTime.Value.ToTimeSpan()) : null,
                    IsRestDay = override_.IsRestDay
                };
            }

            // fall back to weekly pattern
            var scheduleDay = schedule.ScheduleDays
                .FirstOrDefault(x => x.DayOfWeek == date.DayOfWeek);

            if (scheduleDay is null || scheduleDay.IsRestDay)
                return new ScheduledTimeDto
                {
                    StartTime = null,
                    EndTime = null, 
                    IsRestDay = true
                };

            return new ScheduledTimeDto
            {
                StartTime = DateTime.Today.Add(scheduleDay.StartTime!.Value.ToTimeSpan()),
                EndTime = DateTime.Today.Add(scheduleDay.EndTime!.Value.ToTimeSpan()),
                IsRestDay = false
            };
        }
    }
}
