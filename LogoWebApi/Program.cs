using LogoWebApi.Logging;
using System.Diagnostics.CodeAnalysis;


var builder = WebApplication.CreateBuilder(args);

// Initialize logging configuration
LogGenerator.Initialize(builder.Configuration);

// Add hosted service for logging host start and stop
builder.Services.AddHostedService<HostedServiceLogger>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new () { Title = "LogoWebApi", Version = "v1" });
//var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogoWebApi v1");
c.DocumentTitle = "LogoWebApi";
});


app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RequestLoggingMiddleware>();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}

