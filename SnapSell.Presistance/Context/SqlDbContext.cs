using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Models;
using SnapSell.Domain.Models.Interfaces;
using SnapSell.Presistance.Extensions;

namespace SnapSell.Presistance.Context;

public sealed class SqlDbContext(DbContextOptions<SqlDbContext> options, IHttpContextAccessor contextAccessor)
    : IdentityDbContext<User>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Variant> Variants { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderAddress> OrderAddresses { get; set; }
    public DbSet<ShoppingBag> ShoppingBags { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Review> Reviews { get; set; }

    //public DbSet<CacheCode> CacheCodes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Accounts").Property(x => x.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);


        modelBuilder.ApplyGlobalFilters<IAuditable>(x => !x.IsDeleted);
    }

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
            return contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        catch
        {
            return null; 
        }
    }
}