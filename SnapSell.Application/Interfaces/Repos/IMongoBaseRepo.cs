namespace SnapSell.Application.Interfaces.Repos
{
    public interface IMongoBaseRepo<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
    }
}
