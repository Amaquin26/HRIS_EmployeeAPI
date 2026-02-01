using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Services
{
    public interface IEmployeeStatusService
    {
        Task<List<EmployeeStatus>> GetAllEmployeeStatus();
    }
}
