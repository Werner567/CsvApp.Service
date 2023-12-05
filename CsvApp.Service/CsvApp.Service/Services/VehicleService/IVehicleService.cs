using CsvApp.Service.Models;

namespace CsvApp.Service.Services.VehicleService
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehicles(CancellationToken cancellationToken);
        Task<Vehicle> GetVehicleById(Guid id, CancellationToken cancellationToken);
        Task<Guid> AddVehicle(Vehicle vehicle, CancellationToken cancellationToken);
        Task<bool> UpdateVehicle(Vehicle vehicle, CancellationToken cancellationToken);
        Task<bool> DeleteVehicle(Guid id, CancellationToken cancellationToken);
        Task<List<Vehicle>> QueryVehicles(string make, string type, CancellationToken cancellationToken);
    }
}
