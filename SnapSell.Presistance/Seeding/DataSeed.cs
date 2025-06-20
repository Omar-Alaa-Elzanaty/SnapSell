using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Presistance.Context;

namespace SnapSell.Presistance.Seeding;

public class DataSeed
{
    public static async Task SeedDate(IServiceProvider services)
    {
        var context = services.GetRequiredService<SqlDbContext>();
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.Client));
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

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