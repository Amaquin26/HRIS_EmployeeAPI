using HRIS_Employee.API.Repositories;
using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Services
{
    public class EmployeeStatusService(IEmployeeStatusRepository employeeStatusRepository) : IEmployeeStatusService
    {
        public Task<List<EmployeeStatus>> GetAllEmployeeStatus()
        {
            return employeeStatusRepository.GetAllEmployeeStatusAsync();
        }
    }
}
