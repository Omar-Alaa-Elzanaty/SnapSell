﻿using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Presistance.Context;
using System.Linq.Expressions;

namespace SnapSell.Presistance.Repos
{
    public class SQLBaseRepo<T> : ISQLBaseRepo<T> where T : class
    {
        private readonly SqlDbContext _context;
        public SQLBaseRepo(SqlDbContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(nameof(entities));

            await _context.AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _context.Remove(entity);
        }
        public void UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Update(entity);
        }
        public IQueryable<T> Entites => _context.Set<T>();

        public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
            =>  await Task.FromResult(_context.Set<T>().Where(predicate));

        public async Task<T?> FindOnCriteriaAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().FirstOrDefaultAsync(predicate);

        public void UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
        }
    }
}
