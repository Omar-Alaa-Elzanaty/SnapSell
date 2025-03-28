using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Application.Interfaces;
using SnapSell.Infrastructure.Services.ApiRequestService;
using SnapSell.Infrastructure.Services.MediaServices;
using SnapSell.Infrastructure.Services.PaymentGateway;
using System.Net.Http.Headers;

namespace SnapSell.Infrastructure.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services
                .AddServices()
                .AddPaymobService(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IMediaService, LocalMediaService>()
                .AddScoped<IPaymobService, PaymobService>()
                .AddScoped<IApiRequestHandleService, ApiRequestHandleService>();

            return services;
        }

        private static IServiceCollection AddPaymobService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpClient<IPaymobService>("PaymobService", (serviceProvider, httpClient) =>
            {
                httpClient.BaseAddress = new Uri(configuration["Paymob:BaseUrl"]!.ToString());
                httpClient.DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders
                .Add("Authorization", configuration["Paymob:SecretKey"]);
            });

            return services;
        }
    }
}
