using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Slot.Gateway
{
    public class Program
    {
        private const string AppName = "slot-gateway";
        public static void Main(string[] args)
        {
            
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.WithProperty("Application", AppName)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.WithProperty("Environment", env);

            if (env != "Development")
            {
                loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());
            }
            else
            {
                loggerConfiguration.WriteTo.Console();
            }

            Log.Logger = loggerConfiguration.CreateLogger();
            try
            {
                Log.Information($"{AppName} starting up.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{AppName} failed to start.");
            }
            finally
            {
                Log.Information($"{AppName} shutting down.");
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
