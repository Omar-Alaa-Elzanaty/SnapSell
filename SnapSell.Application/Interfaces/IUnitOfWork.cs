using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models.MongoDbEntities;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IBaseRepo<T> Repository<T>() where T : class;
        IMongoBaseRepo<Product> ProductsRepo { get; }
        ISQLBaseRepo<Category> CategoryRepo { get; }
        ISQLBaseRepo<CacheCode> CacheCodesRepo { get; } 
        ISQLBaseRepo<Variant> VariantsRepo { get; set; }
        ISQLBaseRepo<Store> StoresRepo { get; set; }
        ISQLBaseRepo<Client> ClientsRepo { get; set; }
        ISQLBaseRepo<Brand> BrandsRepo { get; set; }
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
