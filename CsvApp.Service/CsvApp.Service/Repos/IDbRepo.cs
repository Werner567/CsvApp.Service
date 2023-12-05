using CsvApp.Service.Models;

namespace CsvApp.Service.Repos
{
    public interface IDbRepo
    {
        Task AddAsync(Vehicle dataModel, CancellationToken cancellationToken);
        Task AddManyAsync(List<Vehicle> dataModel, CancellationToken cancellationToken);
        Task<List<Vehicle>> ReadFilterManyAsync(List<Vehicle> dataModel, CancellationToken cancellationToken);
        Task<List<Vehicle>> ReadManyAsync(CancellationToken cancellationToken);
        Task<Vehicle> ReadOneAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AddOneAsync(Vehicle dataModel, CancellationToken cancellationToken);
        Task<bool> UpdateOneAsync(Vehicle dataModel, CancellationToken cancellationToken);
        Task<bool> DeleteOneAsync(Guid id, CancellationToken cancellationToken);
    }
}
