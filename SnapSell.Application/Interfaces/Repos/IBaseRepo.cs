namespace SnapSell.Application.Interfaces.Repos
{
    public interface IBaseRepo<T>
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        void Delete();
        Task AddRange(IEnumerable<T> entities);
    }
}
