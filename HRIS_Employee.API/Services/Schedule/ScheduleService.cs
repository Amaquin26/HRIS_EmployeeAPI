using HRIS_Employee.API.DTOs.Schedule;
using HRIS_Employee.API.Repositories.Employee;
using HRIS_Employee.API.Repositories.Schedule;
using HRIS_Employee.API.Repositories.ScheduleDay;
using HRIS_Employee.API.Services.UnitOfWork;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;

namespace HRIS_Employee.API.Services.Schedule
{
    public class ScheduleService(IScheduleRepository scheduleRepository, IScheduleDayRepository scheduleDayRepository, IUnitOfWork unitOfWork) : IScheduleService
    {
        public async Task<EmployeeScheduleDto> GetEmployeeSchedule(int employeeId)
        {
            var schedule = await scheduleRepository.GetSingleAsync(
                x => x.Employees.Any(e => e.Id == employeeId),
                query => query.Include(x => x.ScheduleDays));


            if (schedule == null)
            {
                throw new Exception("Schedule not found"); // TODO: custom exception
            }

            var scheduleDto = new EmployeeScheduleDto
            {
                Id = schedule.Id,
                EmployeeId = employeeId,
                BreakMinutes = schedule.BreakMinutes,
                ScheduleDays = schedule.ScheduleDays.Select(x => new ScheduleDayDto
                {
                    Id = x.Id,
                    DayOfWeek = x.DayOfWeek,
                    DayName = x.DayOfWeek.ToString(),
                    StartTime = x.StartTime.HasValue
                    ? DateTime.Today.Add(x.StartTime.Value.ToTimeSpan())
                    : null,
                    EndTime = x.EndTime.HasValue
                    ? DateTime.Today.Add(x.EndTime.Value.ToTimeSpan())
                    : null,
                    CrossesMidnight = x.CrossesMidnight,
                    IsRestDay = x.IsRestDay,

                }).OrderBy(x => x.DayOfWeek).ToList()
            };

            return scheduleDto;
        }

        public async Task<List<ScheduleDayDto>> AddScheduleDays(int employeeId, AddScheduleDaysDto addScheduleDays)
        {
            var schedule = await scheduleRepository.GetSingleAsync(
                x => x.Employees.Any(e => e.Id == employeeId),
                query => query.Include(x => x.ScheduleDays));


            if (schedule == null)
            {
                throw new Exception("Schedule not found"); // TODO: custom exception
            }

            var existingDays = schedule.ScheduleDays;

            var allDays = Enum.GetValues<DayOfWeek>();

            var missingDays = allDays
                .Where(day => existingDays.All(x => x.DayOfWeek != day))
                .ToList();

            if (missingDays.Count == 0)
            {
                throw new Exception("Already have schedule days"); // TODO: custom exception or resolve
            }

            var newScheduleDays = missingDays.Select(day => new ScheduleDay
            {
                ScheduleId = schedule.Id,
                DayOfWeek = day,
                StartTime = addScheduleDays.RestDays.Contains(day) ? null : addScheduleDays.StartTime,
                EndTime = addScheduleDays.RestDays.Contains(day) ? null : addScheduleDays.EndTime,
                CrossesMidnight = false,
                IsRestDay = addScheduleDays.RestDays.Contains(day)
            }).ToList();

            await scheduleRepository.AddScheduleDaysAsync(newScheduleDays);

            await unitOfWork.SaveChangesAsync();

            return newScheduleDays.Select(x => new ScheduleDayDto
            {
                Id = x.Id,
                ScheduleId = x.ScheduleId,
                DayOfWeek = x.DayOfWeek,
                StartTime = x.StartTime.HasValue
                    ? DateTime.Today.Add(x.StartTime.Value.ToTimeSpan())
                    : null,
                EndTime = x.EndTime.HasValue
                    ? DateTime.Today.Add(x.EndTime.Value.ToTimeSpan())
                    : null,
                CrossesMidnight = x.CrossesMidnight,
                IsRestDay = x.IsRestDay
            }).ToList();
        }

        public async Task<List<ScheduleDayDto>> EditScheduleDays(int employeeId, List<ScheduleDayWriteDto> scheduleDays)
        {
            var scheduleDaysId = scheduleDays.Select(x => x.Id);

            var existingScheduleDays = (await scheduleDayRepository.GetAllAsync(
                x => scheduleDaysId.Contains(x.Id) 
                    && x.Schedule.Employees.Any(s => s.Id == employeeId)))
                    .ToDictionary(x => x.Id);

            if (existingScheduleDays.Count != scheduleDays.Count)
            {
                throw new Exception("Some schedule days not found"); // TODO: custom exception
            }

            foreach (var scheduleDay in scheduleDays) {
                existingScheduleDays.TryGetValue(scheduleDay.Id, out var existingScheduleDay);

                if (existingScheduleDay == null)
                {
                    throw new Exception($"Schedule day with ID {scheduleDay.Id} not found"); // TODO: custom exception
                }

                if (!scheduleDay.IsRestDay && (scheduleDay.StartTime == null || scheduleDay.EndTime == null)){
                    throw new Exception($"Start time and end time are required for non-rest days (Schedule Day ID: {scheduleDay.Id})"); // TODO: custom exception
                }

                existingScheduleDay.StartTime = scheduleDay.StartTime;
                existingScheduleDay.EndTime = scheduleDay.EndTime;
                existingScheduleDay.CrossesMidnight = scheduleDay.CrossesMidnight;
                existingScheduleDay.IsRestDay = scheduleDay.IsRestDay;
            }

            await unitOfWork.SaveChangesAsync();

            return existingScheduleDays.Values
                .Select(x => new ScheduleDayDto
                {
                    Id = x.Id,
                    StartTime = x.StartTime.HasValue
                        ? DateTime.Today.Add(x.StartTime.Value.ToTimeSpan())
                        : null,
                    EndTime = x.EndTime.HasValue
                        ? DateTime.Today.Add(x.EndTime.Value.ToTimeSpan())
                        : null,
                    CrossesMidnight = x.CrossesMidnight,
                    IsRestDay = x.IsRestDay,
                    DayOfWeek = x.DayOfWeek,
                    ScheduleId = x.ScheduleId,
                    DayName = x.DayOfWeek.ToString()
                })
                .OrderBy(x => x.DayOfWeek)
                .ToList();
        }
    }
}
