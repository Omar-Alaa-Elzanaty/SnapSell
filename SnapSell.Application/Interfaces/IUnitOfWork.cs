using SnapSell.Application.Interfaces.Repos;

namespace SnapSell.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepo<T> Repository<T>() where T : class;
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
