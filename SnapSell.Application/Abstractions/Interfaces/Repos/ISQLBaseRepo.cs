﻿using System.Linq.Expressions;

namespace SnapSell.Application.Interfaces.Repos;

public interface ISQLBaseRepo<T>
{

    Task AddAsync(T entity);
    Task AddRange(IEnumerable<T> entities);
    void Delete(T entity);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    IQueryable<T> Entities { get; }
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
    Task<T?> FindOnCriteriaAsync(Expression<Func<T, bool>> predicate);
}