using HRIS_Employee.API.DTOs.Schedule;
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
    public class EmployeeScheduleController(IScheduleService scheduleService) : ControllerBase
    {
        [HttpGet("{employeeId:int}")]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetEmployeeSchedule(int employeeId)
        {
            var employeeSchedule = await scheduleService.GetEmployeeSchedule(employeeId);
            return Ok(employeeSchedule);
        }

        [HttpPost("{employeeId:int}")]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> AddScheduleDays (int employeeId, [FromBody] AddScheduleDaysDto addScheduleDaysDto)
        {
            var scheduleDays = await scheduleService.AddScheduleDays(employeeId, addScheduleDaysDto);
            return Ok(scheduleDays);
        }
    }
}
