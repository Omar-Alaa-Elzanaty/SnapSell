using SnapSell.Application.Interfaces.Repos;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance.Repos
{
    public class MongoBaseRepo<T> : IMongoBaseRepo<T> where T : class
    {
        private readonly MongoDbContext _dbContext;

        public MongoBaseRepo(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
        }
    }
}
