using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlDbContext _context;

    public IMongoBaseRepo<Product> ProductsRepo { get; private set; }
    public ISQLBaseRepo<Product> ProductsRepository { get; private set; }
    public ISQLBaseRepo<CacheCode> CacheCodesRepo { get; private set; }

    public UnitOfWork(
        SqlDbContext context,
        IMongoBaseRepo<Product> productRepo,
        ISQLBaseRepo<CacheCode> cacheCodesRepo,
        ISQLBaseRepo<Product> productsRepository)
    {
        _context = context;
        ProductsRepo = productRepo;
        CacheCodesRepo = cacheCodesRepo;
        ProductsRepository = productsRepository;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}