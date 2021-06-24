using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Slot.Gateway.Infrastructure;
using Slot.Gateway.Infrastructure.Middleware;
using Slot.Gateway.Settings;

namespace Slot.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sectionAppSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(sectionAppSettings);
            var appSettings = sectionAppSettings.Get<AppSettings>();


            services
                .AddSlotService(appSettings)
                .AddAggregators()
                .AddServices()
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddApiVersioning(o =>
                {
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                });
            ;

            services.AddHealthChecks();
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Slot.Gateway", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Slot.Gateway v1"));
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = _ => false
                });

                //Should check 3td party or DB helty
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = _ => false
                });
            });
        }
    }
}