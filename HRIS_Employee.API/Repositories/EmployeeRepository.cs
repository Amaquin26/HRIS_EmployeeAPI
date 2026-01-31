using HRIS_Employee.Infrastructure.Persistence.DBContext;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HRIS_Employee.API.Repositories
{
    public class EmployeeRepository(EmployeeDbContext dbContext) : IEmployeeRepository
    {
        public async Task<List<Employee>> GetAllAsync(Expression<Func<Employee, bool>>? predicate = null)
        {
            IQueryable<Employee> query = dbContext.Employees;

            query = query.Include(e => e.EmployeeStatus);

            return await query.ToListAsync();
        }

        public async Task<Employee?> GetSingleByIdAsync(int id, Expression<Func<Employee, bool>>? predicate = null)
        {
            IQueryable<Employee> query = dbContext.Employees;

            if (predicate != null)
                query = query.Where(predicate);

            query = query.Include(e => e.EmployeeStatus);

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetSingleByEntraObjectIdAsync(string entraObjectId, Expression<Func<Employee, bool>>? predicate = null)
        {
            IQueryable<Employee> query = dbContext.Employees;

            if (predicate != null)
                query = query.Where(predicate);

            query = query.Include(e => e.EmployeeStatus);

            return await query.FirstOrDefaultAsync(e => e.EntraObjectId == entraObjectId);
        }
    }
}
