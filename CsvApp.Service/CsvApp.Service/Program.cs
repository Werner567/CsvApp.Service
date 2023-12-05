using Microsoft.AspNetCore.Hosting;

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
            }).ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
            });
    }
}