using CsvApp.Service.Models;
using CsvApp.Service.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;

namespace CsvApp.Service.Repos
{
    public class DbRepo : IDbRepo
    {
        private readonly SqliteOptions _options;
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<DbRepo> _logger;

        public DbRepo(IOptions<SqliteOptions> options, AppDbContext appDbContext, ILogger<DbRepo> logger)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<List<Vehicle>> ReadFilterManyAsync(List<Vehicle> dataModel, CancellationToken cancellationToken)
        {
            var allVehicles = await _appDbContext.Vehicles.ToListAsync(cancellationToken);
            // Extract Make and Model from the provided list of vehicles
            // var makeModelPairs = dataModel.Select(vehicle => new { vehicle.Make, vehicle.Model }).ToList();

            // Query the database for existing vehicles with matching Make and Model
            if (allVehicles.Count() == 0)
            {
                return null;
            }
            var existingVehicles = allVehicles
            .Where(vehicle => dataModel.Any(pair =>
            pair.Type == vehicle.Type &&
            pair.Make == vehicle.Make &&
            pair.Model == vehicle.Model &&
            pair.Year == vehicle.Year &&
            pair.WheelCount == vehicle.WheelCount &&
            pair.FuelType == vehicle.FuelType &&
            pair.Active == vehicle.Active))
        .ToList();

            return existingVehicles;
        }

        public async Task<List<Vehicle>> ReadManyAsync(CancellationToken cancellationToken)
        {

            return await _appDbContext.Vehicles.ToListAsync(cancellationToken);

        }

        public async Task<Vehicle> ReadOneAsync(Guid id, CancellationToken cancellationToken)
        {
            var data = await _appDbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return data;
        }



        public async Task AddAsync(Vehicle dataModel, CancellationToken cancellationToken)
        {
            _appDbContext.Vehicles.Add(dataModel);
        }

        public async Task AddManyAsync(List<Vehicle> dataModel, CancellationToken cancellationToken)
        {
            foreach (var dataModelItem in dataModel)
            {
                _appDbContext.Vehicles.Add(dataModelItem);
            }
            _appDbContext.SaveChanges();
        }

        public async Task<bool> DeleteOneAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var vehicleToDelete = await _appDbContext.Vehicles.FindAsync(id);

                if (vehicleToDelete == null)
                {
                    // The vehicle with the specified id was not found
                    return false;
                }

                _appDbContext.Vehicles.Remove(vehicleToDelete);
                await _appDbContext.SaveChangesAsync(cancellationToken);

                return true; // Vehicle deleted successfully
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return false; // Return false to indicate failure
            }
        }

        public async Task<bool> UpdateOneAsync(Vehicle dataModel, CancellationToken cancellationToken)
        {
            try
            {
                // Find the existing vehicle in the database by Id
                var existingVehicle = await _appDbContext.Vehicles.FindAsync(dataModel.Id);

                if (existingVehicle == null)
                {
                    // The vehicle with the specified Id was not found
                    return false;
                }

                // Update the properties of the existing vehicle with the new values
                existingVehicle.Type = dataModel.Type;
                existingVehicle.Make = dataModel.Make;
                existingVehicle.Model = dataModel.Model;
                existingVehicle.Year = dataModel.Year;
                existingVehicle.WheelCount = dataModel.WheelCount;
                existingVehicle.FuelType = dataModel.FuelType;
                existingVehicle.Active = dataModel.Active;

                // Save the changes to the database
                await _appDbContext.SaveChangesAsync(cancellationToken);

                return true; // Vehicle updated successfully
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return false; // Return false to indicate failure
            }
        }
        public async Task<bool> AddOneAsync(Vehicle dataModel, CancellationToken cancellationToken)
        {
            try
            {
                _appDbContext.Vehicles.Add(dataModel);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }



    }
}
