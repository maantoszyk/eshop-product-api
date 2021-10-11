using Eshop.Product.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;


namespace Eshop.Product.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using var host = CreateHostBuilder(args).Build();

                // Seed the Database
                {
                    using var scope = host.Services.CreateScope();
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<ProductDbContext>();
                    _ = ProductDbInitializer.Initialize(context);
                }

                host.Run();
            }
            catch (Exception ex)
            {
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                }
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext();
                    });
                });
    }
}
