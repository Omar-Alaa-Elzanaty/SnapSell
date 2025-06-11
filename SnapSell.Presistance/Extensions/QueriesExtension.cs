using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SnapSell.Domain.Models.Interfaces;

namespace SnapSell.Presistance.Extensions;

public static class QueriesExtension
{
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder,
        Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newbody =
                ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
        }
    }

    public static TEntity ApplyAuditableEntities<TEntity>(
        this TEntity entity,
        EntityState state, 
        IHttpContextAccessor httpContextAccessor) where TEntity : IAuditable
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var now = DateTime.UtcNow;

        if (state is EntityState.Added)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = userId;
            entity.IsDeleted = false;
            entity.LastUpdatedAt = now;
            entity.LastUpdatedBy = userId;
        }
        else if (state is EntityState.Modified)
        {
            entity.LastUpdatedAt = now;
            entity.LastUpdatedBy = userId;
        }
        else if (state is EntityState.Deleted)
        {
            entity.IsDeleted = true;
            entity.LastUpdatedAt = now;
            entity.LastUpdatedBy = userId;
        }

        return entity;
    }
}