using HRIS_Employee.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HRIS_Employee.API.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
    {
        protected readonly EmployeeDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(EmployeeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
