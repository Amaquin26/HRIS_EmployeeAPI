using HRIS_Employee.API.DTOs.Schedule;
using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Services.Schedule
{
    public interface IScheduleService
    {
        Task<EmployeeScheduleDto> GetEmployeeSchedule(int employeeId);

        Task<List<ScheduleDayDto>> AddScheduleDays(int employeeId, AddScheduleDaysDto addScheduleDays);

        Task<List<ScheduleDayDto>> EditScheduleDays(int employeeId, List<ScheduleDayWriteDto> scheduleDays);
    }
}
