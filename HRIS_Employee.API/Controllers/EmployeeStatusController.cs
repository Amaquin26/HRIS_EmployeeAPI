using HRIS_Employee.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeStatusController(IEmployeeStatusService employeeStatusService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployeeStatus()
        {
            var employeeStatuses = await employeeStatusService.GetAllEmployeeStatus();
            return Ok(employeeStatuses);
        }
    }
}
