using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Repos;
using System.Collections;

namespace SnapSell.Presistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlDbContext _context;

        public IMongoBaseRepo<Product> ProductsRepo { get; private set; }

        public UnitOfWork(
            SqlDbContext context,
            IMongoBaseRepo<Product> productRepo)
        {
            _context = context;
            ProductsRepo = productRepo;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
