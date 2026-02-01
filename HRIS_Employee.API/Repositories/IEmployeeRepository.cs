using HRIS_Employee.Infrastructure.Persistence.Models;
using System.Linq.Expressions;

namespace HRIS_Employee.API.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync(Expression<Func<Employee, bool>>? predicate = null);

        Task<(List<Employee>, int)> GetAllPaginatedAsync(int pageSize = 10, int pageNumber = 1, string? searchTerm = "");

        Task<Employee?> GetSingleByIdAsync(int id, Expression<Func<Employee, bool>>? predicate = null);

        Task<Employee?> GetSingleByEntraObjectIdAsync(string entraObjectId, Expression<Func<Employee, bool>>? predicate = null);

        Task<bool> AddEmployee(Employee employee);

        Task<string?> GetLastEmployeeNumber();
    }
}
