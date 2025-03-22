using Serilog;
using SnapSell.API;
using SnapSell.Application.Extensions.Services;
using SnapSell.Infrastructure.Services;
using SnapSell.Presentation.MiddleWare;
using SnapSell.Presistance.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeFormatServices());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services
       .AddApplication()
       .AddPresistance()
       .AddInfrastructure()
       .DepedencyInjectionService(builder.Configuration);

builder.Host.UseSerilog((context, config) =>
config.ReadFrom.Configuration(context.Configuration));

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
