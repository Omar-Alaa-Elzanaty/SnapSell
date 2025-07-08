using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Models.MongoDbEntities;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
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
                .AddSqlDbContext(configuration)
                .AddMongoDbContext(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddScoped(typeof(ISQLBaseRepo<>), typeof(SqlBaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>()
                    .AddScoped(typeof(IMongoBaseRepo<>),typeof(MongoBaseRepo<>));

            return services;
        }

        private static IServiceCollection AddSqlDbContext(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<SqlDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(
                     configuration.GetConnectionString("DbConnection"),
                         sqlOptions => sqlOptions.MigrationsAssembly(typeof(SqlDbContext).Assembly.FullName)
                ));

            services.AddIdentity<Account, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager<SignInManager<Account>>()
                    .AddUserManager<UserManager<Account>>()
                    .AddEntityFrameworkStores<SqlDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection AddMongoDbContext(this IServiceCollection services, 
            IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));

            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(sp.GetRequiredService<IMongoDbSettings>().ConnectionString));

            services.AddScoped<MongoDbContext>();
            services.AddMongoCollection<Product>();

            return services;
        }

        private static IServiceCollection AddMongoCollection<T>(this IServiceCollection services)
        {
            return services.AddScoped(sp =>
            {
                var context = sp.GetRequiredService<MongoDbContext>();
                return context.GetCollection<T>();
            });
        }
    }
}
