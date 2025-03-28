using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Interfaces;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Extensions;
using System.Security.Claims;

namespace SnapSell.Presistance.Context
{
    public class SnapSellDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SnapSellDbContext(
            DbContextOptions<SnapSellDbContext> options,
            IHttpContextAccessor contextAccessor) : base(options)
        {
            _contextAccessor = contextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("Account");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SnapSellDbContext).Assembly);

            modelBuilder.ApplyGlobalFilters<IAuditable>(x => !x.IsDeleted);
        }

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
