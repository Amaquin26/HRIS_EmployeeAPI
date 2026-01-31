using HRIS_Employee.API.DTOs;

namespace HRIS_Employee.API.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto?> GetEmployeeByEntraObjectIdAsync(string entraObjectId);
    }
}
