using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Repos;
using System.Configuration;

namespace SnapSell.Presistance.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddServices(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddDbContext<SnapSellDbContext>(options =>
                 options.UseSqlServer(
                     configuration.GetConnectionString("DbConnection"),
                         sqlOptions => sqlOptions.MigrationsAssembly(typeof(SnapSellDbContext).Assembly.FullName)
                 ));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<SnapSellDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
