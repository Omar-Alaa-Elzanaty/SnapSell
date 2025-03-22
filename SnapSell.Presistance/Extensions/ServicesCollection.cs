using Microsoft.Extensions.DependencyInjection;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Presistance.Repos;

namespace SnapSell.Presistance.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddPresistance(this IServiceCollection services)
        {
            services
                .AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>))
                    .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
