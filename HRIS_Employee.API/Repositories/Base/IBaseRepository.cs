using System.Linq.Expressions;

namespace HRIS_Employee.API.Repositories.Base
{
    public interface IBaseRepository<TEntity>
    where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        Task<TEntity?> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        Task AddAsync(TEntity entity);

        void UpdateAsync(TEntity entity);

        void DeleteAsync(TEntity entity);
    }
}
