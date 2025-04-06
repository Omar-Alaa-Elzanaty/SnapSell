using Microsoft.Extensions.Options;
using Serilog;
using SnapSell.API;
using SnapSell.Application.Extensions.Services;
using SnapSell.Infrastructure.Extensions;
using SnapSell.Infrastructure.Services.JsonSerilizeServices;
using SnapSell.Presentation.MiddleWare;
using SnapSell.Presistance.Extensions;
using SnapSell.Presistance.Seeding;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeFormatService());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services
       .AddInfrastructure(builder.Configuration)
       .AddPresistance(builder.Configuration)
       .AddApplication()
       .DepedencyInjectionService(builder.Configuration);

var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

Log.Logger = logger;
builder.Host.UseSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseRouting();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleWare>();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

DataSeed.SeedDate(app.Services.CreateScope().ServiceProvider).Wait();

app.Run();
