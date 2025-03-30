using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Presistance.Seeding
{
    public class DataSeed
    {
        public static async Task SeedDate(IServiceProvider services)
        {
            var context = services.GetRequiredService<SnapSellDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                await roleManager.CreateAsync(new IdentityRole(Roles.Client));

                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                var admin = new ApplicationUser()
                {
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    UserName= "admin"
                };

                await userManager.CreateAsync(admin, "123@Abc");
            }
        }
    }
}
