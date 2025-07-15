using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance.Seeding;

public class DataSeed
{
    public static async Task SeedData(IServiceProvider services)
    {
        var context = services.GetRequiredService<SqlDbContext>();

        var migrationsCount = context.Database.GetMigrations().Count();
        var pendingMigrationsCount = context.Database.GetPendingMigrations().Count();

        if (pendingMigrationsCount > 0)
        {
            await context.Database.MigrateAsync();

            if (migrationsCount == pendingMigrationsCount)
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                await roleManager.CreateAsync(new IdentityRole(Roles.Client));
                var userManager = services.GetRequiredService<UserManager<Account>>();

                var admin = new Account()
                {
                    FullName = "admin",
                    Email = "admin@gmail.com",
                    UserName = "admin"
                };

                await userManager.CreateAsync(admin, "123@Abc");
                await context.SaveChangesAsync();
            }
        }
    }
}