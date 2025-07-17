using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlDbContext _context;
    public ISQLBaseRepo<Product> ProductsRepo { get; private set; }
    public ISQLBaseRepo<ProductCategory> ProductCategoriesRepo { get; }
    public ISQLBaseRepo<Category> CategoryRepo { get; private set; }
    public ISQLBaseRepo<CacheCode> CacheCodesRepo { get; private set; }
    public ISQLBaseRepo<Variant> VariantsRepo { get;private set; }
    public ISQLBaseRepo<Store> StoresRepo { get; private set; }
    public ISQLBaseRepo<Client> ClientsRepo { get; private set; }
    public ISQLBaseRepo<Brand> BrandsRepo { get; private set; }
    public ISQLBaseRepo<Size> SizesRepo { get; private set; }
    public ISQLBaseRepo<OrderAddress> OrderAddressesRepo { get; private set; }

    public UnitOfWork(
        SqlDbContext context,
        ISQLBaseRepo<CacheCode> cacheCodesRepo,
        ISQLBaseRepo<Product> productsRepo,
        ISQLBaseRepo<Variant> variants,
        ISQLBaseRepo<Store> stores,
        ISQLBaseRepo<Client> clients,
        ISQLBaseRepo<Brand> brands,
        ISQLBaseRepo<Category> categoryRepo,
        ISQLBaseRepo<Size> sizesRepo,
        ISQLBaseRepo<ProductCategory> productCategoryRepo,
        ISQLBaseRepo<OrderAddress> orderAddress)
    {
        _context = context;
        CacheCodesRepo = cacheCodesRepo;
        ProductsRepo = productsRepo;
        VariantsRepo = variants;
        StoresRepo = stores;
        ClientsRepo = clients;
        BrandsRepo = brands;
        CategoryRepo = categoryRepo;
        SizesRepo = sizesRepo;
        ProductCategoriesRepo = productCategoryRepo;
        OrderAddressesRepo = orderAddress;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}