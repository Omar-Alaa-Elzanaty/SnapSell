using SnapSell.Application.Interfaces.Repos;

namespace SnapSell.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveAsync();
        IBaseRepo<T> Repository<T>();
    }
}
