using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapSell.Application.Interfaces;
using SnapSell.Infrastructure.Services.ApiRequestService;
using SnapSell.Infrastructure.Services.MailServices;
using SnapSell.Infrastructure.Services.MediaServices;
using SnapSell.Infrastructure.Services.NotificationServices;
using SnapSell.Infrastructure.Services.PaymentGateway;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace SnapSell.Infrastructure.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddServices()
                .AddFluentEmailServices(configuration)
                .AddPaymobService(configuration)
                .AddNotificationService(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IMediaService, LocalMediaService>()
                .AddScoped<IPaymobService, PaymobService>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IApiRequestHandleService, ApiRequestHandleService>();

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

        private static IServiceCollection AddNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("private_key.json")
                });
            }

            
            services.AddScoped<INotificationSender, NotificationSender>();
            return services;
        }
    }
}
