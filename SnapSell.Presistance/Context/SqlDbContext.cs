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

    //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    var userId = contextAccessor.HttpContext is null || contextAccessor.HttpContext.User is null
    //        ? null
    //        : contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    //    foreach (var entity in ChangeTracker
    //                 .Entries()
    //                 .Where(x => x.Entity is IAuditable
    //                             && (x.State == EntityState.Added ||
    //                                 x.State == EntityState.Modified ||
    //                                 x.State == EntityState.Deleted)))
    //    {
    //        IAuditable auditedEntity = (entity as IAuditable)!;

    //        if (entity.State == EntityState.Added)
    //        {
    //            auditedEntity.CreatedAt = DateTime.UtcNow;

    //            if (userId != null)
    //            {
    //                auditedEntity.CreatedBy = userId;
    //            }
    //        }
    //        else
    //        {
    //            if (entity.State == EntityState.Deleted)
    //            {
    //                auditedEntity.IsDeleted = true;
    //                entity.State = EntityState.Modified;
    //            }

    //            auditedEntity.LastUpdatedAt = DateTime.UtcNow;

    //            if (userId != null)
    //            {
    //                auditedEntity.LastUpdatedBy = userId;
    //            }
    //        }
    //    }
    //    return await base.SaveChangesAsync(cancellationToken);
    //}
}