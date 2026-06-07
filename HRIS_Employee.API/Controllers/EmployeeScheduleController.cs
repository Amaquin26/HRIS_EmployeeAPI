using HRIS_Employee.API.DTOs.Schedule;
using HRIS_Employee.API.Services;
using HRIS_Employee.API.Services.Schedule;
using HRIS_Employee.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeScheduleController(IScheduleService scheduleService, IShiftRecordService shiftRecordService) : BaseController
    {
        [HttpGet("{employeeId:int}")]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetEmployeeSchedule(int employeeId)
        {
            var employeeSchedule = await scheduleService.GetEmployeeSchedule(employeeId);
            return Ok(employeeSchedule);
        }

        [HttpGet("shift-detail")]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> GetShiftDetail()
        {
           var entraObjectId = GetUserOID();

            if (entraObjectId == null)
                return Unauthorized("OID claim not found.");

            var result = await shiftRecordService.GetEmployeeShiftDetailAsync(entraObjectId);
            return Ok(result);
        }

        [HttpPost("{employeeId:int}")]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> AddScheduleDays (int employeeId, [FromBody] AddScheduleDaysDto addScheduleDaysDto)
        {
            var scheduleDays = await scheduleService.AddScheduleDays(employeeId, addScheduleDaysDto);
            return Ok(scheduleDays);
        }

        [HttpPost("clock-in")]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> ClockIn()
        {
            var entraObjectId = GetUserOID();

            if (entraObjectId == null)
                return Unauthorized("OID claim not found.");

            var result = await shiftRecordService.ClockInAsync(entraObjectId);
            return Ok(result);
        }

        [HttpPost("clock-out")]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> ClockOut()
        {
            var entraObjectId = GetUserOID();

            if (entraObjectId == null)
                return Unauthorized("OID claim not found.");

            var result = await shiftRecordService.ClockOutAsync(entraObjectId);
            return Ok(result);
        }

        [HttpPost("clock-out/undo")]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> UndoClockOut()
        {
            var entraObjectId = GetUserOID();

            if (entraObjectId == null)
                return Unauthorized("OID claim not found.");

            var result = await shiftRecordService.UndoClockOutAsync(entraObjectId);
            return Ok(result);
        }

        [HttpPut("{employeeId:int}/schedule-days")]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> EditScheduleDays(int employeeId, [FromBody] List<ScheduleDayWriteDto> scheduleDays)
        {
            var result = await scheduleService.EditScheduleDays(employeeId, scheduleDays);
            return Ok(result);
        }
    }
}
