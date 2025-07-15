using Microsoft.Extensions.Options;
using Serilog;
using SnapSell.API;
using SnapSell.Application.Extensions.Services;
using SnapSell.Infrastructure.Extnesions;
using SnapSell.Infrastructure.Services.JsonSerilizeServices;
using SnapSell.Presentation.MiddleWare;
using SnapSell.Presistance.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeFormatService());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddPresistance(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .DepedencyInjectionService(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

Log.Logger = logger;
builder.Host.UseSerilog(logger);

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("DEVELOPMENT");
}
else
{
    app.UseCors("PRODUCTION");
}

var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions?.Value!);

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     await services.EnsureProductTextIndexCreatedAsync();
// }

app.UseRouting();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleWare>();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

DataSeed.SeedData(app.Services.CreateScope().ServiceProvider).Wait();

app.Run();
