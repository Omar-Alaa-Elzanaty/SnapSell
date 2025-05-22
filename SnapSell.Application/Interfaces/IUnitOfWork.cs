using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IBaseRepo<T> Repository<T>() where T : class;
        ISQLBaseRepo<Product> ProductsRepository { get; }
        ISQLBaseRepo<CacheCode> CacheCodesRepo { get; } 
        ISQLBaseRepo<Variant> Variants { get; set; }
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
