using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<T> query = tracking ? _dbContext.Set<T>() : _dbContext.Set<T>().AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) => await _dbContext.Set<T>().FindAsync(id);

        public async Task AddAsync(T entity) => await _dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity) => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => _dbContext.Set<T>().AsNoTracking().AnyAsync(predicate, ct);

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<T> query = tracking ? _dbContext.Set<T>() : _dbContext.Set<T>().AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate);

        }

    }
}
