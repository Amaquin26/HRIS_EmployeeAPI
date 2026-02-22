using HRIS_Employee.API.DTOs;
using HRIS_Employee.API.External.Graph;
using HRIS_Employee.API.Services;
using HRIS_Employee.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController(IEmployeeService employeeService, IGraphService graphService) : ControllerBase
    {
        [HttpGet]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("paginated/result")]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetAllEmployeesPaginated([FromQuery] PaginationQueryDto paginationQuery)
        {
            var result = await employeeService.GetAllEmployeesPaginated(paginationQuery);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"No Employee found with ID: {id}"
                });

            return Ok(employee);
        }

        [HttpGet("oid/{entraObjectId}")]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetEmployeeByEntraObjectId(string entraObjectId)
        {
            var employee = await employeeService.GetEmployeeByEntraObjectIdAsync(entraObjectId);
            if (employee == null)
                return NotFound($"No Employee found with Entra Object ID: {entraObjectId}");

            return Ok(employee);
        }

        [HttpGet("graph")]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetGraphUser([FromQuery] string UserPrincipalName)
        {
            var graphUser = await graphService.GetUser(UserPrincipalName);
            if (graphUser == null)
                return NotFound($"No Graph user found with principal name: {UserPrincipalName}");

            return Ok(graphUser);
        }

        [HttpGet("graph/paginated")]
        [RequiredScope(ApiScopeConstants.EmployeesRead)]
        public async Task<IActionResult> GetPaginatedGraphUsers([FromQuery] PaginationQueryDto pagination)
        {
            var graphUsers = await graphService.GetPaginatedUsers(pagination);

            return Ok(graphUsers);
        }

        [HttpPost]
        [RequiredScope(ApiScopeConstants.EmployeesWrite)]
        public async Task<IActionResult> AddEmployeeRecord([FromBody] WriteEmployeeDto employeeDto)
        {
            var employee = await employeeService.AddEmployeeRecord(employeeDto);
            return Ok();
        }
    }
}
