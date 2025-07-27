using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Abstractions.Interfaces.Repos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlDbContext _context;
    public ISQLBaseRepo<Product> ProductsRepo { get; private set; }
    public ISQLBaseRepo<Account> AccountsRepo { get; }
    public ISQLBaseRepo<ProductImage> ProductImagesRepo { get; }
    public ISQLBaseRepo<ProductCategory> ProductCategoriesRepo { get; }
    public ISQLBaseRepo<Category> CategoryRepo { get; private set; }
    public ISQLBaseRepo<CacheCode> CacheCodesRepo { get; private set; }
    public ISQLBaseRepo<Variant> VariantsRepo { get; private set; }
    public ISQLBaseRepo<Store> StoresRepo { get; private set; }
    public ISQLBaseRepo<Brand> BrandsRepo { get; private set; }
    public ISQLBaseRepo<Size> SizesRepo { get; private set; }
    public ISQLBaseRepo<Order> OrdersRepo { get; private set; }
    public ISQLBaseRepo<OrderAddress> OrderAddressesRepo { get; private set; }
    public ISQLBaseRepo<Seller> SellersRepo { get; private set; }

    public UnitOfWork(
        SqlDbContext context,
        ISQLBaseRepo<CacheCode> cacheCodesRepo,
        ISQLBaseRepo<Product> productsRepo,
        ISQLBaseRepo<Variant> variants,
        ISQLBaseRepo<Store> stores,
        ISQLBaseRepo<Brand> brands,
        ISQLBaseRepo<Category> categoryRepo,
        ISQLBaseRepo<Size> sizesRepo,
        ISQLBaseRepo<ProductCategory> productCategoryRepo,
        ISQLBaseRepo<OrderAddress> orderAddressRepo,
        ISQLBaseRepo<Order> orderRepo, ISQLBaseRepo<ProductImage> productImagesRepo,
        ISQLBaseRepo<Account> accountsRepo,
        ISQLBaseRepo<Seller> seller)
    {
        _context = context;
        CacheCodesRepo = cacheCodesRepo;
        ProductsRepo = productsRepo;
        VariantsRepo = variants;
        StoresRepo = stores;
        BrandsRepo = brands;
        CategoryRepo = categoryRepo;
        SizesRepo = sizesRepo;
        ProductCategoriesRepo = productCategoryRepo;
        OrderAddressesRepo = orderAddressRepo;
        OrdersRepo = orderRepo;
        ProductImagesRepo = productImagesRepo;
        AccountsRepo = accountsRepo;
        SellersRepo = seller;
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