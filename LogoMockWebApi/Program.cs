using LogoMockWebApi;
using Serilog;
using System.Diagnostics;

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

LogHelper.Initialize(configuration);

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHostedService<IISServiceWatcher>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    
    app.UseSwagger();
    app.UseSwaggerUI();


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseMiddleware<RequestLoggingMiddleware>();

    app.MapControllers();

    app.Run();
} 
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Closing web host");
    Log.CloseAndFlush();
}

