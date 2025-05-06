using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Interfaces;
using SnapSell.Application.Interfaces.Authentication;
using SnapSell.Infrastructure.Services.ApiRequestService;
using SnapSell.Infrastructure.Services.Authentication;
using SnapSell.Infrastructure.Services.CacheServices;
using SnapSell.Infrastructure.Services.I18nServices;
using SnapSell.Infrastructure.Services.MailServices;
using SnapSell.Infrastructure.Services.MediaServices;
using SnapSell.Infrastructure.Services.PaymentGateway;
using SnapSell.Infrastructure.Services.PushNotificationServices;
using JwtSettings = SnapSell.Infrastructure.Services.Authentication.JwtSettings;

namespace SnapSell.Infrastructure.Extnesions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddServices()
                .AddFluentEmailServices(configuration)
                .AddPaymobService(configuration)
                .AddPushNotificationService(configuration)
                .AddI18NService()
                .AddJwt(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IMediaService, LocalMediaService>()
                .AddScoped<IPaymobService, PaymobService>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IPushNotificationSender, PushNotificationSender>()
                .AddScoped<IApiRequestHandleService, ApiRequestHandleService>()
                .AddScoped<ICacheService, CacheService>()
                .AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>()
                .AddScoped<IVideoUploadService, VideoUploadService>();

            return services;
        }

        private static IServiceCollection AddPaymobService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IPaymobService>("PaymobService", (serviceProvider, httpClient) =>
            {
                httpClient.BaseAddress = new Uri(configuration["Paymob:BaseUrl"]!.ToString());
                httpClient.DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders
                .Add("Authorization", "TOKEN " + configuration["Paymob:SecretKey"]);
            });

            return services;
        }

        private static IServiceCollection AddFluentEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("MailSettings");
            var defaultFromEmail = emailSettings["SenderEmail"];
            var host = emailSettings["Server"];
            var port = emailSettings.GetValue<int>("Port");
            var userName = emailSettings["UserName"];
            var password = emailSettings["Password"];
            var enableSsl = emailSettings.GetValue<bool>("EnableSSL");
            var useDefaultCredentials = emailSettings.GetValue<bool>("UseDefaultCredentials");
            var senderName = emailSettings["SenderName"];

            var smtpClient = new SmtpClient
            {
                EnableSsl = enableSsl,
                Host = host,
                Port = port,
                UseDefaultCredentials = useDefaultCredentials,
                Credentials = new NetworkCredential(userName, password),
                Timeout = 200000
            };

            services.AddFluentEmail(defaultFromEmail, senderName)
                    .AddRazorRenderer()
                    .AddLiquidRenderer()
                    .AddSmtpSender(smtpClient);

            return services;
        }

        private static IServiceCollection AddPushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("private_key.json")
                });
            }

            return services;
        }

        private static IServiceCollection AddI18NService(this IServiceCollection services)
        {

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            return services;
        }
        private static IServiceCollection AddJwt(this IServiceCollection services,IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            JwtSettings jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
