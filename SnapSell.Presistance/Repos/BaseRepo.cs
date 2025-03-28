using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Presistance.Context;
using System.Linq.Expressions;

namespace SnapSell.Presistance.Repos
{
    class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly SnapSellDbContext _snapSellDbContext;
        public BaseRepo(SnapSellDbContext context) => _snapSellDbContext = context ?? throw new ArgumentNullException(nameof(context));
        public async Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _snapSellDbContext.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            await _snapSellDbContext.AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _snapSellDbContext.Remove(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _snapSellDbContext.Update(entity);
        }
        public IQueryable<T> Entites() => _snapSellDbContext.Set<T>();
        public async Task<T?> GetByIdAsync(Guid id) => await _snapSellDbContext.Set<T>().FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _snapSellDbContext.Set<T>().ToListAsync();
        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
            =>  await Task.FromResult(_snapSellDbContext.Set<T>().Where(predicate));
        public async Task<T?> FindOnCriteriaAsync(Expression<Func<T, bool>> predicate)
            => await _snapSellDbContext.Set<T>().FirstOrDefaultAsync(predicate);
       
    }
}
