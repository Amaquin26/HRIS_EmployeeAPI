using HRIS_Employee.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.API.Services.UnitOfWork
{
    public class UnitOfWork(EmployeeDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        } 
    }
}
