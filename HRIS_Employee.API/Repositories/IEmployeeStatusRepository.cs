using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Repositories
{
    public interface IEmployeeStatusRepository
    {
        Task<List<EmployeeStatus>> GetAllEmployeeStatusAsync();
    }
}
