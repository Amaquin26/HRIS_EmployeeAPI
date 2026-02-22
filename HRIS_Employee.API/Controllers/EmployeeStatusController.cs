using HRIS_Employee.API.Services;
using HRIS_Employee.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeStatusController(IEmployeeStatusService employeeStatusService) : ControllerBase
    {
        [HttpGet]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetEmployeeStatus()
        {
            var employeeStatuses = await employeeStatusService.GetAllEmployeeStatus();
            return Ok(employeeStatuses);
        }
    }
}
