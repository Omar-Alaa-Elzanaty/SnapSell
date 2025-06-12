using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Extensions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace SnapSell.Presistance.Repos;

public class MongoBaseRepo<T>(
    IHttpContextAccessor httpContextAccessor,
    MongoDbContext dbContext,
    IMongoCollection<T> collection,
    string collectionName = null)
    : IMongoBaseRepo<T>
    where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection = dbContext.GetCollection<T>(collectionName);
    public IMongoCollection<T> Collection { get; } = collection;

    public IQueryable<T> Entities => _collection.AsQueryable();

    public virtual async Task InsertOneAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.ApplyAuditableEntities(EntityState.Added, httpContextAccessor);
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentException("Entities collection cannot be null or empty", nameof(entities));

        await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
    }

    public virtual async Task<T> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<T> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<T> FindOneAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(FilterDefinition<T>.Empty).ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter,
        CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public virtual async Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public virtual async Task<bool> UpdateOneAsync(T entity, CancellationToken cancellationToken = default)
    {
        var id = GetIdValue(entity);
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public virtual async Task<bool> DeleteAsync(FilterDefinition<T> filter,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public virtual async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public virtual async Task<long> CountAsync(FilterDefinition<T> filter,
        CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    public virtual async Task<long> CountAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(FilterDefinition<T> filter,
        CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    private object GetIdValue(T entity)
    {
        var idProperty = typeof(T).GetProperties()
            .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                                 p.Name.Equals("_id", StringComparison.OrdinalIgnoreCase));

        if (idProperty == null)
            throw new InvalidOperationException("Entity must have an Id property");

        return idProperty.GetValue(entity);
    }
}