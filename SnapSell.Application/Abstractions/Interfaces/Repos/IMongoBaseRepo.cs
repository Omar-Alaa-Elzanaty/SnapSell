using System.Linq.Expressions;
using MongoDB.Driver;
using SnapSell.Domain.Models.Interfaces;

namespace SnapSell.Application.Interfaces.Repos;

public interface IMongoBaseRepo<T> where T : IAuditable
{
    // Collection access
    IMongoCollection<T> Collection { get; }

    // Create operations
    Task InsertOneAsync(T entity, CancellationToken cancellationToken = default);
    Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    // Read operations
    Task<T> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<T> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<T> FindOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    // Update operations
    Task<bool> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateOneAsync(T entity, CancellationToken cancellationToken = default);

    // Delete operations
    Task<bool> DeleteAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default);

    // Utility operations
    Task<long> CountAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<long> CountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}