using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IBaseRepo<T> Repository<T>() where T : class;
        IMongoBaseRepo<Product> ProductsRepo { get; }
        ISQLBaseRepo<CacheCode> CacheCodesRepo { get; }
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
