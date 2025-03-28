using System.Linq.Expressions;

namespace SnapSell.Application.Interfaces.Repos
{
    public interface IBaseRepo<T>
    {
        Task AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        Task UpdateAsync(T entity);
        IQueryable<T> Entites();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        Task<T?> FindOnCriteriaAsync(Expression<Func<T, bool>> predicate);
    }
}
