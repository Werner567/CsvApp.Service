using CsvApp.Service.Models;
using CsvApp.Service.Options;
using CsvApp.Service.Services.CsvLoaderService;
using Microsoft.Extensions.Options;

namespace CsvApp.Service.Services.CsvDataLoaderWorker
{
    public class CsvDataLoaderWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly ICsvLoaderService _csvLoaderoptions;
        private readonly ILogger<CsvDataLoaderWorker> _logger;
        private readonly ServiceOptions _serviceOptions;
        private List<Vehicle> _vehicles;
        //private readonly IServiceScopeFactory _scopeFactory;

        public CsvDataLoaderWorker(IServiceProvider serviceProvider, IOptions<ServiceOptions> serviceOptions/*, ICsvLoaderService csvLoaderoptions,*/ ,ILogger<CsvDataLoaderWorker> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
           // _csvLoaderoptions = csvLoaderoptions ?? throw new ArgumentNullException(nameof(csvLoaderoptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceOptions = serviceOptions.Value ?? throw new ArgumentNullException(nameof(serviceOptions));
            _vehicles = new List<Vehicle>();
           // _scopeFactory = scopeFactory;
        }
        public List<Vehicle> GetLoadedData()
        {

            return _vehicles;
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

                        // Use csvLoaderService...
                        await csvLoaderService.LoadCsv(filePath, stoppingToken); // Replace with your actual method call
                    }

                   // await _csvLoaderoptions.LoadCsv(filePath, stoppingToken);
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
