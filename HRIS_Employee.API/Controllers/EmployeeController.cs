using HRIS_Employee.API.DTOs;
using HRIS_Employee.API.External.Graph;
using HRIS_Employee.API.Services;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService, IGraphService graphService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("paginated/result")]
        public async Task<IActionResult> GetAllEmployeesPaginated([FromQuery] PaginationQueryDto paginationQuery)
        {
            var result = await employeeService.GetAllEmployeesPaginated(paginationQuery);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound($"No Employee found with ID: {id}");

            return Ok(employee);
        }

        [HttpGet("oid/{entraObjectId}")]
        public async Task<IActionResult> GetEmployeeByEntraObjectId(string entraObjectId)
        {
            var employee = await employeeService.GetEmployeeByEntraObjectIdAsync(entraObjectId);
            if (employee == null)
                return NotFound($"No Employee found with Entra Object ID: {entraObjectId}");

            return Ok(employee);
        }

        [HttpGet("graph")]
        public async Task<IActionResult> GetGraphUser([FromQuery] string UserPrincipalName)
        {
            var graphUser = await graphService.GetUser(UserPrincipalName);
            if (graphUser == null)
                return NotFound($"No Graph user found with principal name: {UserPrincipalName}");

            return Ok(graphUser);
        }

        [HttpGet("graph/paginated")]
        public async Task<IActionResult> GetPaginatedGraphUsers([FromQuery] PaginationQueryDto pagination)
        {
            var graphUsers = await graphService.GetPaginatedUsers(pagination);

            return Ok(graphUsers);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeRecord([FromBody] WriteEmployeeDto employeeDto)
        {
            var employee = await employeeService.AddEmployeeRecord(employeeDto);
            return Ok();
        }
    }
}
