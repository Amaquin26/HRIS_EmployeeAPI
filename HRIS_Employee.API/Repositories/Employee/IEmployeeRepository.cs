using HRIS_Employee.API.Repositories.Base;
using System.Linq.Expressions;

namespace HRIS_Employee.API.Repositories.Employee
{
    public interface IEmployeeRepository : IBaseRepository<Infrastructure.Persistence.Models.Employee>
    {
        Task<(List<Infrastructure.Persistence.Models.Employee>, int)> GetAllPaginatedAsync(int pageSize = 10, int pageNumber = 1, string? searchTerm = "");

        Task<Infrastructure.Persistence.Models.Employee?> GetSingleByEntraObjectIdAsync(string entraObjectId, Expression<Func<Infrastructure.Persistence.Models.Employee, bool>>? predicate = null);

        Task<string?> GetLastEmployeeNumber();
    }
}
