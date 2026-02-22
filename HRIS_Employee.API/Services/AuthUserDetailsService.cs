using HRIS_Employee.API.DTOs;
using HRIS_Employee.API.Repositories;
using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Services
{
    public class AuthUserDetailsService(IEmployeeRepository employeeRepository) : IAuthUserDetailsService
    {
        public async Task<AuthUserDetailsDto?> GetMyUserDetails(string entraObjectId)
        {
            var employee = await employeeRepository.GetSingleByEntraObjectIdAsync(entraObjectId);

            if (employee == null) return null;

            return new AuthUserDetailsDto
            {
                Id = employee.Id,
                EntraObjectId = employee.EntraObjectId,
                EmployeeNumber = employee.EmployeeNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName
            };
        }
    }
}
