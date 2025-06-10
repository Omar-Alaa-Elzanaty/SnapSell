using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using SnapSell.Application.Extensions.Services;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Models.SqlEntities;
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
        protected MongoDbRunner _mongoDbRunner;

        public TestBase()
        {
            _builder = WebApplication.CreateBuilder();
            _builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            AddSQLLiteDbContext(_builder.Services, _builder.Configuration);
            _builder.Services
                .AddApplication()
                .AddInfrastructure(_builder.Configuration)
                .AddMemoryCache();

            //AddMongoDbContext(_builder.Services, _builder.Configuration);
            SeedDateAsync().GetAwaiter();
        }

        private void AddSQLLiteDbContext(IServiceCollection services, IConfiguration configuration, IHttpContextAccessor contextAccessor = null)
        {
            var connection = CreateDatabaseAndGetConnection(contextAccessor);
            services.AddDbContext<SqlDbContext>(options =>
                          options.UseSqlite(connection,
                              _builder => _builder.MigrationsAssembly(typeof(SqlDbContext).Assembly.FullName)), ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            if (contextAccessor is not null)
            {
                services.AddSingleton(contextAccessor);
                _serviceProvider = _builder.Services.BuildServiceProvider();
            }

            services.AddIdentity<Account, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager<SignInManager<Account>>()
                    .AddUserManager<UserManager<Account>>()
                    .AddEntityFrameworkStores<SqlDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped(typeof(ISQLBaseRepo<>), typeof(SqlBaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>();
        }

        //private void AddMongoDbContext(IServiceCollection services, IConfiguration configuration)
        //{
        //    _mongoDbRunner = MongoDbRunner.Start();
        //    var mongoSetting = new MongoClient(_mongoDbRunner.ConnectionString);

        //    services.AddDbContext<MongoDbContext>(options =>
        //            options.UseMongoDB(mongoSetting, configuration["MongoSetting:Database"]!));

        //    services.AddScoped(typeof(IMongoBaseRepo<>), typeof(MongoBaseRepo<>));
        //}

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
                var roleManger = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = _serviceProvider.GetRequiredService<UserManager<Account>>();

                var admin = new Account()
                {
                    FullName = "admin",
                    Email = "admin@gmail.com",
                    UserName = "admin"
                };

                await userManager.CreateAsync(admin, "123@Abc");

                await roleManger.CreateAsync(new(Roles.Admin));
                await roleManger.CreateAsync(new(Roles.Client));
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
