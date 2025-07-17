using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Abstractions.Interfaces;

public interface IUnitOfWork : IDisposable
{
    //IBaseRepo<T> Repository<T>() where T : class;
    ISQLBaseRepo<Product> ProductsRepo { get; }
    ISQLBaseRepo<ProductCategory> ProductCategoriesRepo { get; }
    ISQLBaseRepo<Category> CategoryRepo { get; }
    ISQLBaseRepo<CacheCode> CacheCodesRepo { get; }
    ISQLBaseRepo<Variant> VariantsRepo { get; }
    ISQLBaseRepo<Store> StoresRepo { get; }
    ISQLBaseRepo<Client> ClientsRepo { get; }
    ISQLBaseRepo<Brand> BrandsRepo { get; }
    ISQLBaseRepo<Size> SizesRepo { get; }
    ISQLBaseRepo<OrderAddress> OrderAddressesRepo { get; }
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}