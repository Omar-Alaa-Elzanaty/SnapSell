using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Interfaces;
using SnapSell.Domain.Models;
using System.Security.Claims;

namespace SnapSell.Presistance.Context
{
    public class MongoDbContext : DbContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public MongoDbContext(
            DbContextOptions options,
            IHttpContextAccessor contextAccessor) : base(options)
        {
            _contextAccessor = contextAccessor;
        }

        public DbSet<Product> Products { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _contextAccessor.HttpContext is null || _contextAccessor.HttpContext.User is null ?
                null : _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            foreach (var entity in ChangeTracker
            .Entries()
                .Where(x => x.Entity is IAuditable
                && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)))
            {
                IAuditable auditedEntity = (entity as IAuditable)!;

                if (entity.State == EntityState.Added)
                {
                    auditedEntity.CreatedAt = DateTime.UtcNow;

                    if (userId != null)
                    {
                        auditedEntity.CreatedBy = userId;
                    }
                }
                else
                {
                    if (entity.State == EntityState.Deleted)
                    {
                        auditedEntity.IsDeleted = true;
                        entity.State = EntityState.Modified;
                    }

                    auditedEntity.LastUpdatedAt = DateTime.UtcNow;

                    if (userId != null)
                    {
                        auditedEntity.LastUpdatedBy = userId;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
