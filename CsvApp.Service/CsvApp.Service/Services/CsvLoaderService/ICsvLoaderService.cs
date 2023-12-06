using CsvApp.Service.Models;

namespace CsvApp.Service.Services.CsvLoaderService
{
    public interface ICsvLoaderService
    {
        Task LoadCsv(string filePath, CancellationToken cancellationToken);
    }
}
