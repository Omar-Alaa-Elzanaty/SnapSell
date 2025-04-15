using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Entities;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Context.SnapSell.Infrastructure.Data;
using SnapSell.Presistance.Repos;

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

            // Add MongoDB configuration
            var mongoDbSettings = configuration.GetSection("MongoDB");
            services.AddSingleton<IMongoClient>(serviceProvider =>
                new MongoClient(mongoDbSettings["ConnectionString"]));

            services.AddScoped<MongoDbContext>(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return new MongoDbContext(client, mongoDbSettings["DatabaseName"]!);
            });
          
            // Optional: Create indexes on startup
            //services.AddHostedService<MongoIndexService>();


            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SnapSellDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
