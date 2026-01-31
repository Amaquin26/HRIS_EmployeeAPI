using HRIS_Employee.API.DTOs;
using HRIS_Employee.API.Repositories;

namespace HRIS_Employee.API.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
    {
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await employeeRepository.GetAllAsync();
            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                employeeDtos.Add(new EmployeeDto
                {
                    Id = employee.Id,
                    EntraObjectId = employee.EntraObjectId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    ContactNumber = employee.ContactNumber,
                    EmployeeStatusId = employee.EmployeeStatusId,
                    EmployeeStatusName = employee.EmployeeStatus.Name
                });
            }

            return employeeDtos;
        }

        public async Task<EmployeeDto?> GetEmployeeByEntraObjectIdAsync(string entraObjectId)
        {
            var employee = await employeeRepository.GetSingleByEntraObjectIdAsync(entraObjectId);

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                EntraObjectId = employee.EntraObjectId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                ContactNumber = employee.ContactNumber,
                EmployeeStatusId = employee.EmployeeStatusId,
                EmployeeStatusName = employee.EmployeeStatus.Name
            };
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await employeeRepository.GetSingleByIdAsync(id);

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                EntraObjectId = employee.EntraObjectId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                ContactNumber = employee.ContactNumber,
                EmployeeStatusId = employee.EmployeeStatusId,
                EmployeeStatusName = employee.EmployeeStatus.Name
            };
        }
    }
}
