using SnapSell.Application.Interfaces.Repos;

namespace SnapSell.Presistance.Repos
{
    class BaseRepo<T> : IBaseRepo<T>
    {
        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
