using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using System.Collections;

namespace SnapSell.Presistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;

        public UnitOfWork()
        {
            _repositories = [];
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IBaseRepo<T> Repository<T>()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
