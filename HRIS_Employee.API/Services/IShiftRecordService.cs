using HRIS_Employee.API.DTOs.Schedule;
using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Services
{
    public interface IShiftRecordService
    {
        Task<ShiftRecordDto> ClockInAsync(string entraObjectId);
        Task<ShiftRecordDto> ClockOutAsync(string entraObjectId);
        Task<ShiftRecordDto> UndoClockOutAsync(string entraObjectId);
        Task<EmployeeScheduleDetailDto> GetEmployeeShiftDetailAsync(string entraObjectId);
    }
}
