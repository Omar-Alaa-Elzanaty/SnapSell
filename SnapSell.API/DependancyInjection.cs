using Microsoft.OpenApi.Models;

namespace SnapSell.API
{
    public static class DependancyInjection
    {
        public static IServiceCollection DepedencyInjectionService(this IServiceCollection services, IConfiguration config)
        {
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PSolve API", Version = "v1.0" });
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Description = "Please enter token",
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        BearerFormat = "JWT",
            //        Scheme = "bearer"
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type=ReferenceType.SecurityScheme,
            //                    Id="Bearer"
            //                }
            //            },
            //            new string[]{}
            //        }
            //    });
            //});

            services
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .AddCors(config);

            return services;
        }

        private static IServiceCollection AddCors(this IServiceCollection services, IConfiguration config)
        {
            var originCors = config.GetSection("APP:CorsOrigins").Get<string>()?.Split(';') ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy("DEVELOPMENT", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });

                options.AddPolicy("PRODUCTION", builder =>
                {
                    builder.WithOrigins(originCors)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });


            return services;
        }
    }
}
