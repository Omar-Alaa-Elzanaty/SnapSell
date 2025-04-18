using Microsoft.Extensions.Options;
using Serilog;
using SnapSell.API;
using SnapSell.Infrastructure.Extensions;
using SnapSell.Infrastructure.JsonSerilizeServices;
using SnapSell.Presentation.MiddleWare;
using SnapSell.Presistance.Extensions;


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

app.Run();

