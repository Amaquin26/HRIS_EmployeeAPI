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

        public async Task<(List<Employee>, int)> GetAllPaginatedAsync(int pageSize = 10, int pageNumber = 1, string? searchTerm = "")
        {
            IQueryable<Employee> query = dbContext.Employees;

            query = query.Include(e => e.EmployeeStatus);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string pattern = $"%{searchTerm}%";
                query = query.Where(e =>
                    EF.Functions.Like(e.FirstName, pattern) ||
                    EF.Functions.Like(e.LastName, pattern) ||
                    EF.Functions.Like(e.Email, pattern) ||
                    EF.Functions.Like(e.EmployeeNumber, pattern)
                );
            }

            int totalRecords = await query.CountAsync();

            query = query
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

            var employees = await query.ToListAsync();

            return (employees, totalRecords);
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

        public async Task<bool> AddEmployee(Employee employee)
        {
            dbContext.Employees.Add(employee);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<string?> GetLastEmployeeNumber()
        {
            var lastEmployeeNumber = await dbContext.Employees
                .OrderByDescending(e => e.EmployeeNumber)
                .Select(e => e.EmployeeNumber)
                .FirstOrDefaultAsync();

            return lastEmployeeNumber;
        }
    }
}
