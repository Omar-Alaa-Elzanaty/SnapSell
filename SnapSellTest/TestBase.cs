using DnsClient.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Moq;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions.Services;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Infrastructure.Extnesions;
using SnapSell.Presistance;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Repos;

namespace SnapSell.Test
{
    public class TestBase : IDisposable
    {
        protected WebApplicationBuilder _builder;
        protected IServiceProvider _serviceProvider;
        protected IServiceScopeFactory _serviceScopeFactory;
        protected MongoDbRunner _mongoDbRunner;

        public TestBase()
        {
            _builder = WebApplication.CreateBuilder();
            _builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var services = new ServiceCollection();

            AddSQLLiteDbContext(services, _builder.Configuration);
            services
                .AddApplication()
                .AddInfrastructure(_builder.Configuration)
                .AddMemoryCache();

            AddMongoDbContext(services, _builder.Configuration);

            _serviceProvider = services.BuildServiceProvider();
            _serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            SeedDateAsync().GetAwaiter();
        }

        private void AddSQLLiteDbContext(ServiceCollection services, IConfiguration configuration, IHttpContextAccessor contextAccessor = null)
        {
            var connection = CreateDatabaseAndGetConnection(contextAccessor);
            services.AddDbContext<SqlDbContext>(options =>
                          options.UseSqlite(connection,
                              _builder => _builder.MigrationsAssembly(typeof(SqlDbContext).Assembly.FullName)).LogTo(Console.WriteLine), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            if (contextAccessor is not null)
            {
                services.AddSingleton(contextAccessor);
            }


            services.AddIdentity<Account, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager<SignInManager<Account>>()
                    .AddUserManager<UserManager<Account>>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<SqlDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped(typeof(ISQLBaseRepo<>), typeof(SqlBaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>()
                    .AddScoped(typeof(IMongoBaseRepo<>), typeof(MongoBaseRepo<>));
        }

        private void AddMongoDbContext(IServiceCollection services, IConfiguration configuration)
        {

            try
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            }
            catch (BsonSerializationException ex) when (ex.Message.Contains("already a serializer registered"))
            {
                
            }

            services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));

            _mongoDbRunner = MongoDbRunner.Start();
            var mongoClient = new MongoClient(_mongoDbRunner.ConnectionString);

            services.AddSingleton<IMongoDbSettings>(sp =>
            sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddScoped(provider =>
                provider.GetService<IMongoClient>()!.GetDatabase(configuration["MongoSetting:Database"]!));

            services.AddScoped<MongoDbContext>();
            services.AddScoped(sp =>
            {
                var context = sp.GetRequiredService<MongoDbContext>();
                return context.GetCollection<Product>();
            });
        }

        protected static SqliteConnection CreateDatabaseAndGetConnection(IHttpContextAccessor contextAccessor = null)
        {
            var http = contextAccessor ?? new Mock<IHttpContextAccessor>().Object;
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            new SqlDbContext(
                new DbContextOptionsBuilder<SqlDbContext>().UseSqlite(connection).Options,
                http).GetService<IRelationalDatabaseCreator>().CreateTables();

            return connection;
        }


        private async Task SeedDateAsync()
        {
            try
            {

                using var scope = _serviceScopeFactory.CreateScope();
                var scopedProvider = scope.ServiceProvider;

                var context = scopedProvider.GetRequiredService<SqlDbContext>();

                var roleManger = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scopedProvider.GetRequiredService<UserManager<Account>>();

                var admin = new Account()
                {
                    FullName = "admin",
                    Email = "admin@gmail.com",
                    UserName = "admin"
                };

                await userManager.CreateAsync(admin, "123@Abc");

                await roleManger.CreateAsync(new(Roles.Admin));
                await roleManger.CreateAsync(new(Roles.Client));
                await roleManger.CreateAsync(new(Roles.Seller));
            }
            catch
            {

            }
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public void Dispose()
        {
            using var context = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<SqlDbContext>();

            context.Database.EnsureDeleted();
            context.Dispose();
            GC.SuppressFinalize(this);

            _mongoDbRunner.Dispose();
        }

        ~TestBase()
        {
            Dispose();
        }
    }
}
