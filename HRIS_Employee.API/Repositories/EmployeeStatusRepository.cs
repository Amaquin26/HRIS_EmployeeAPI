using HRIS_Employee.Infrastructure.Persistence.DBContext;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.API.Repositories
{
    public class EmployeeStatusRepository(EmployeeDbContext dbContext) : IEmployeeStatusRepository
    {
        public Task<List<EmployeeStatus>> GetAllEmployeeStatusAsync()
        {
            return dbContext.EmployeeStatuses.ToListAsync();
        }
    }
}
