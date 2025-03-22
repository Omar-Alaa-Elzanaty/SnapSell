using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Application.Behaviors;
using System.Reflection;

<<<<<<<< HEAD:SnapSell.Application/Extensions/Services/ServicesCollection.cs
namespace SnapSell.Application.Extensions.Services
========
namespace SnapSell.Application.Extensions
>>>>>>>> f8e9f08bb4c74f9f530b3c75e0d44ebd1ca87b37:SnapSell.Application/Extensions/ServicesCollection.cs
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator()
                    .AddMapping()
                    .AddFluentValidation();

            return services;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            //return services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ServicesCollection).Assembly);
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }

        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            services.AddSingleton<IMapper, ServiceMapper>();

            return services;
        }
    }
}
