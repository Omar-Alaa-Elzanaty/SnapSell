using SnapSell.Application.Abstractions.Interfaces.Repos;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Application.Abstractions.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISQLBaseRepo<Product> ProductsRepo { get; }
    ISQLBaseRepo<Account> AccountsRepo { get; }
    ISQLBaseRepo<ProductImage> ProductImagesRepo { get; }
    ISQLBaseRepo<ProductCategory> ProductCategoriesRepo { get; }
    ISQLBaseRepo<Category> CategoryRepo { get; }
    ISQLBaseRepo<CacheCode> CacheCodesRepo { get; }
    ISQLBaseRepo<Variant> VariantsRepo { get; }
    ISQLBaseRepo<Store> StoresRepo { get; }
    ISQLBaseRepo<Client> ClientsRepo { get; }
    ISQLBaseRepo<Seller> SellersRepo { get; }
    ISQLBaseRepo<Brand> BrandsRepo { get; }
    ISQLBaseRepo<Size> SizesRepo { get; }
    ISQLBaseRepo<Order> OrdersRepo { get; }
    ISQLBaseRepo<OrderAddress> OrderAddressesRepo { get; }
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}