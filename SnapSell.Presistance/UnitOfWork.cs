using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlDbContext _context;

    public ISQLBaseRepo<Product> ProductsRepo { get; private set; }
    public ISQLBaseRepo<CacheCode> CacheCodesRepo { get; private set; }
    public ISQLBaseRepo<Variant> VariantsRepo { get; set; }
    public ISQLBaseRepo<Store> StoresRepo { get; set; }
    public ISQLBaseRepo<Client> ClientsRepo { get; set; }
    public ISQLBaseRepo<Brand> BrandsRepo { get; set; }

    public UnitOfWork(
        SqlDbContext context,
        ISQLBaseRepo<CacheCode> cacheCodesRepo,
        ISQLBaseRepo<Product> productsRepo,
        ISQLBaseRepo<Variant> variants,
        ISQLBaseRepo<Store> stores,
        ISQLBaseRepo<Client> clients,
        ISQLBaseRepo<Brand> brands)
    {
        _context = context;
        CacheCodesRepo = cacheCodesRepo;
        ProductsRepo = productsRepo;
        VariantsRepo = variants;
        StoresRepo = stores;
        ClientsRepo = clients;
        BrandsRepo = brands;
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