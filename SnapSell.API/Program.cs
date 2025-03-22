using Serilog;
using SnapSell.API;
<<<<<<< HEAD
using SnapSell.Application.Extensions.Services;
using SnapSell.Infrastructure.Services;
=======
using SnapSell.Application.Extensions;
using SnapSell.Infrastructure.Extensions;
using SnapSell.Infrastructure.JsonSerilizeServices;
>>>>>>> f8e9f08bb4c74f9f530b3c75e0d44ebd1ca87b37
using SnapSell.Presentation.MiddleWare;
using SnapSell.Presistance.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeFormatService());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services
       .AddApplication()
       .AddPresistance()
       .AddInfrastructure()
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

app.UseRouting();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleWare>();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
