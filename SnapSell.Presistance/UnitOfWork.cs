using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlDbContext _context;
    private readonly MongoDbContext _mongoDbContext;

    public ISQLBaseRepo<Product> ProductsRepository { get; private set; }
    public ISQLBaseRepo<CacheCode> CacheCodesRepo { get; private set; }
    public ISQLBaseRepo<Variant> Variants { get; set; }

    public UnitOfWork(
        SqlDbContext context,
        MongoDbContext mongoDbContext,
        ISQLBaseRepo<CacheCode> cacheCodesRepo,
        ISQLBaseRepo<Product> productsRepository,
        ISQLBaseRepo<Variant> variants)
    {
        _context = context;
        CacheCodesRepo = cacheCodesRepo;
        ProductsRepository = productsRepository;
        Variants = variants;
        _mongoDbContext = mongoDbContext;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) +
               await _mongoDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}