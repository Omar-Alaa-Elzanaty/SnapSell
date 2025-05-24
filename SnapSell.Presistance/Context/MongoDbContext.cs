using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using SnapSell.Domain.Models;
using SnapSell.Domain.Models.Interfaces;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToCollection("Products");
        }

        public DbSet<Product> Products { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = GetCurrentUserId();

                foreach (var entry in ChangeTracker.Entries<IAuditable>())
                {
                    if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                        continue;

                    var now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.IsDeleted = false;
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.LastUpdatedAt = now;
                        entry.Entity.LastUpdatedBy = userId;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastUpdatedAt = now;
                        entry.Entity.LastUpdatedBy = userId;
                    }
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string? GetCurrentUserId()
        {
            try
            {
                return _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
