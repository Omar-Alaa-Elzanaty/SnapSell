﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Linq.Expressions;
using System.Reflection;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Extensions;

public static class QuerableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        string? message = null)
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = await source.CountAsync(cancellationToken: cancellationToken);
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return await PaginatedResult<T>.SuccessAsync(items, count, pageNumber, pageSize, message);
    }
    
    public static async Task<List<TEntity>> TextSearchAsync<TEntity>(
        this IMongoCollection<TEntity> collection,
        string searchText,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Text(searchText);
        
        var options = new FindOptions<TEntity>
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize
        };
        
        using var cursor = await collection.FindAsync(
            filter: filter,
            options: options,
            cancellationToken: cancellationToken);
        
        return await cursor.ToListAsync(cancellationToken);
    }

    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<M,T>(
        this IMongoCollection<M> collection,
        FilterDefinition<M> filter,
        SortDefinition<M> sort,
        ProjectionDefinition<M, T>? projection,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var count = await collection.CountDocumentsAsync(filter: filter, cancellationToken: cancellationToken);
        
        var items = await collection.Find(filter)
            .Sort(sort)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .Project(projection)
            .ToListAsync(cancellationToken);

        return await PaginatedResult<T>.SuccessAsync(items, (int)count, pageNumber, pageSize);
    }

    public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string sorting)
    {
        if (string.IsNullOrWhiteSpace(sorting))
        {
            return query;
        }

        var fields = sorting.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
        bool first = true;
        foreach (string field in fields)
        {
            bool ascending = true;
            int spaceIndex = field.IndexOf(' ');
            string key = field;
            if (spaceIndex >= 0)
            {
                key = field.Substring(0, spaceIndex);
                string dir = field.Substring(spaceIndex + 1).Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    switch (dir)
                    {
                        case "asc":
                            ascending = true;
                            break;
                        case "desc":
                            ascending = false;
                            break;
                        default:
                            throw new ArgumentException("Invalid sorting direction");
                    }
                }
            }

            query = ascending
                ? ApplyOrder(query, key, first ? "OrderBy" : "ThenBy")
                : ApplyOrder(query, key, first ? "OrderByDescending" : "ThenByDescending");
            first = false;
        }

        return query;
    }

    static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
    {
        string[] props = property.Split('.');
        Type type = typeof(T);
        ParameterExpression arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach (string prop in props)
        {
            PropertyInfo pi = type.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)!;
            if (pi is null)
            {
                throw new ArgumentException("Invalid sorting property!");
            }
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }
        Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

        object result = typeof(Queryable).GetMethods().Single(method
                => method.Name == methodName
                   && method.IsGenericMethodDefinition
                   && method.GetGenericArguments().Length == 2
                   && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, [source, lambda])!;

        return (IOrderedQueryable<T>)result!;
    }
}