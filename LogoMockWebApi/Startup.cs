using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace LogoMockWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogHelper.Initialize(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Configure Swagger/OpenAPI
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LogoMockWebApi API", Version = "v1" });
            });

            // Other services configuration can be added here as needed
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            if (appLifetime != null)
            {
                appLifetime.ApplicationStopping.Register(OnApplicationStopping);
            }
            else
            {
                Log.Warning("IHostApplicationLifetime is null, some functionalities may not work correctly.");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Enable Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogoMockWebApi V1");
            });

            // Serilog request logging middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void OnApplicationStopping()
        {
            Log.Information("Application is stopping. Host is shutting down.");
            Log.CloseAndFlush();
        }
    }
}