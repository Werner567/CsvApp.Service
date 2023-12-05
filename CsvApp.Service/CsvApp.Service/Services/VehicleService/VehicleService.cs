using CsvApp.Service.Models;
using CsvApp.Service.Repos;
using CsvApp.Service.Services.CsvLoaderService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;

namespace CsvApp.Service.Services.VehicleService
{
    public class VehicleService : IVehicleService
    {
        private readonly List<Vehicle> _vehicles;
        private readonly ICsvLoaderService _csvDataLoaderService;
        private readonly AppDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbRepo _dbRepo;

        //private readonly IServiceScopeFactory _scopeFactory;

        public VehicleService(IDbRepo dbRepo)
        {
            _dbRepo = dbRepo ?? throw new ArgumentNullException(nameof(dbRepo));
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehicles(CancellationToken cancellationToken)
        {
            return await _dbRepo.ReadManyAsync(cancellationToken);
        }

        public async Task<Vehicle> GetVehicleById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbRepo.ReadOneAsync(id, cancellationToken);
        }

        public async Task<Guid> AddVehicle(Vehicle vehicle, CancellationToken cancellationToken)
        {
            var result = await _dbRepo.AddOneAsync(vehicle, cancellationToken);
            if (!result)
            {
                return Guid.Empty;
            }
            return vehicle.Id;
        }

        public async Task<bool> UpdateVehicle(Vehicle vehicle, CancellationToken cancellationToken)
        {
            if (vehicle == null)
            {                
                return false;
            }

            return await _dbRepo.UpdateOneAsync(vehicle, cancellationToken); ;
        }

        public async Task<bool> DeleteVehicle(Guid id, CancellationToken cancellationToken)
        {
            return await _dbRepo.DeleteOneAsync(id, cancellationToken);

        }

        public async Task<List<Vehicle>> QueryVehicles(string make, string type, CancellationToken cancellationToken)
        {
            return await _dbRepo.ReadFilterAsync(make,type, cancellationToken);
        }
    }
}
