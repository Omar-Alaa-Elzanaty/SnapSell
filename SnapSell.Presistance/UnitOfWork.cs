using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Repos;
using System.Collections;

namespace SnapSell.Presistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SnapSellDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(SnapSellDbContext context)
        {
            _context = context;
            _repositories = [];
        }

        public IBaseRepo<T> Repository<T>() where T : class
        {
            _repositories ??= [];

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepo<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepo<T>)_repositories[type]!;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
