using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Application.Interfaces;
using SnapSell.Infrastructure.MediaServices;

namespace SnapSell.Infrastructure.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IMediaService, LocalMediaService>();

            return services;
        }
    }
}
