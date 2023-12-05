using CsvApp.Service.Models;

namespace CsvApp.Service.Services.CsvLoaderService
{
    public interface ICsvLoaderService
    {
        List<Vehicle> GetLoadedData();
        Task LoadCsv(string filePath, CancellationToken cancellationToken);
    }
}
