using CsvApp.Service.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net;

namespace CsvApp.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(currentDirectory, "Data/Config");
                config.SetBasePath(configPath);

                foreach (var configFile in Directory.GetFiles(configPath, "*.json"))
                {
                    config.AddJsonFile(Path.GetFileName(configFile), optional: true, reloadOnChange: true);
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                int servicePort = 5000;
                webBuilder.ConfigureKestrel(options =>
                {
                    var configuration = options.ApplicationServices.GetRequiredService<IConfiguration>();
                    var serviceConfig = configuration.GetSection("ServiceOptions").Get<ServiceOptions>();
                    servicePort = serviceConfig.ServicePort;

                });
                webBuilder.UseUrls($"https://localhost:{servicePort}");

            }).ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
            });


    }
}