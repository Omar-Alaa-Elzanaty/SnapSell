using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models.MongoDbEntities;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlDbContext _context;
    public IMongoBaseRepo<Product> ProductsRepo { get; private set; }
    public ISQLBaseRepo<Category> CategoryRepo { get; private set; }
    public ISQLBaseRepo<CacheCode> CacheCodesRepo { get; private set; }
    public ISQLBaseRepo<Variant> VariantsRepo { get; set; }
    public ISQLBaseRepo<Store> StoresRepo { get; set; }
    public ISQLBaseRepo<Client> ClientsRepo { get; set; }
    public ISQLBaseRepo<Brand> BrandsRepo { get; set; }
    public ISQLBaseRepo<Size> SizesRepo { get; set; }

    public UnitOfWork(
        SqlDbContext context,
        ISQLBaseRepo<CacheCode> cacheCodesRepo,
        IMongoBaseRepo<Product> productsRepo,
        ISQLBaseRepo<Variant> variants,
        ISQLBaseRepo<Store> stores,
        ISQLBaseRepo<Client> clients,
        ISQLBaseRepo<Brand> brands,
        ISQLBaseRepo<Category> categoryRepo,
        ISQLBaseRepo<Size> sizesRepo)
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