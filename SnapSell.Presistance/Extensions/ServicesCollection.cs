using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Repos;

namespace SnapSell.Presistance.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddServices(configuration)
                .AddSQLDbContext(configuration);
                //.AddMongoDbContext(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddScoped(typeof(ISQLBaseRepo<>), typeof(SQLBaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>()
                    .AddScoped(typeof(IMongoBaseRepo<>),typeof(MongoBaseRepo<>));

            return services;
        }

        private static IServiceCollection AddSQLDbContext(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<SqlDbContext>(options =>
                options.UseSqlServer(
                     configuration.GetConnectionString("DbConnection"),
                         sqlOptions => sqlOptions.MigrationsAssembly(typeof(SqlDbContext).Assembly.FullName)
                ));

            services.AddIdentity<User, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddUserManager<UserManager<User>>()
                    .AddEntityFrameworkStores<SqlDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }

        //private static IServiceCollection AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
        //{

        //    var mongoSetting = new MongoClient(configuration["MongoSetting:Connection"]);

        //    services.AddDbContext<MongoDbContext>(options =>
        //            options.UseMongoDB(mongoSetting, configuration["MongoSetting:Database"]!));

        //    return services;
        //}
    }
}
