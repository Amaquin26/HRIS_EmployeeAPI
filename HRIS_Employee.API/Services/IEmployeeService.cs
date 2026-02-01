using HRIS_Employee.API.DTOs;
using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto?> GetEmployeeByEntraObjectIdAsync(string entraObjectId);

        Task<Employee> AddEmployeeRecord(WriteEmployeeDto employeeDto);
    }
}
