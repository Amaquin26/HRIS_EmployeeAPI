using HRIS_Employee.API.DTOs.Schedule;
using HRIS_Employee.API.Repositories.Employee;
using HRIS_Employee.API.Repositories.Schedule;
using HRIS_Employee.API.Services.UnitOfWork;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.API.Services.Schedule
{
    public class ScheduleService( IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork) : IScheduleService
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
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    CrossesMidnight = x.CrossesMidnight,
                    IsRestDay = x.IsRestDay,

                }).ToList()
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
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                CrossesMidnight = x.CrossesMidnight,
                IsRestDay = x.IsRestDay
            }).ToList();
        }
    }
}
