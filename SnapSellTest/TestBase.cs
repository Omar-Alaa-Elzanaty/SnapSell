using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SnapSell.Application.Extensions.Services;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Constants;
using SnapSell.Domain.Models;
using SnapSell.Infrastructure.Extensions;
using SnapSell.Presistance;
using SnapSell.Presistance.Context;
using SnapSell.Presistance.Repos;

namespace SnapSell.Test
{
    internal class TestBase : IDisposable
    {
        protected WebApplicationBuilder _builder;
        protected IServiceProvider _serviceProvider;

        public TestBase()
        {
            _builder = WebApplication.CreateBuilder();
            _builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            AddSQLLiteDbContext(_builder.Services, _builder.Configuration);
            _builder.Services
                .AddApplication()
                .AddInfrastructure(_builder.Configuration)
                .AddMemoryCache();

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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager<SignInManager<ApplicationUser>>()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddEntityFrameworkStores<SqlDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped(typeof(ISQLBaseRepo<>), typeof(BaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>();
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
                var roleManger = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var admin = new ApplicationUser()
                {
                    FirstName = "admin",
                    LastName = "admin",
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

        ~TestBase()
        {
            Dispose();
        }

        public void Dispose()
        {
            using var context = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<SqlDbContext>();

            context.Database.EnsureDeleted();
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
