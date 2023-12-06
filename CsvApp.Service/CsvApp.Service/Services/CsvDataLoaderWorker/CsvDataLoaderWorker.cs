using CsvApp.Service.Models;
using CsvApp.Service.Options;
using CsvApp.Service.Services.CsvLoaderService;
using Microsoft.Extensions.Options;

namespace CsvApp.Service.Services.CsvDataLoaderWorker
{
    public class CsvDataLoaderWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CsvDataLoaderWorker> _logger;
        private readonly ServiceOptions _serviceOptions;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="serviceOptions"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CsvDataLoaderWorker(IServiceProvider serviceProvider, IOptions<ServiceOptions> serviceOptions,ILogger<CsvDataLoaderWorker> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceOptions = serviceOptions.Value ?? throw new ArgumentNullException(nameof(serviceOptions));
        }
                

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var filePath = Path.Combine(@$"{Directory.GetCurrentDirectory()}\Data\CSV", _serviceOptions.CsvFileName);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var csvLoaderService = scope.ServiceProvider.GetRequiredService<ICsvLoaderService>();

                        await csvLoaderService.LoadCsv(filePath, stoppingToken); 
                    }
                    // Sleep or delay before the next iteration
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}
