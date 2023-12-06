using CsvApp.Service.Models;
using CsvApp.Service.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CsvApp.Service.Repos
{
    public class DbRepo : IDbRepo
    {
        private readonly IOptions< VehicleOptions> _vehicleOptions;
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<DbRepo> _logger;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="VehicleOptions"></param>
        /// <param name="appDbContext"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DbRepo(IOptions<SqliteOptions> options, IOptions<VehicleOptions> VehicleOptions, AppDbContext appDbContext, ILogger<DbRepo> logger)
        {
            _vehicleOptions = VehicleOptions ?? throw new ArgumentNullException(nameof(VehicleOptions));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Queries the DB for new changes from CSV
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Vehicle>> ReadFilterManyAsync(List<Vehicle> dataModel, CancellationToken cancellationToken)
        {
            var allVehicles = await _appDbContext.Vehicles.ToListAsync(cancellationToken);
           
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

        /// <summary>
        /// Queries DB with a filter by make and type
        /// </summary>
        /// <param name="make"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Vehicle>> ReadFilterAsync(string make, string type, CancellationToken cancellationToken)
        {
            var allVehicles = await _appDbContext.Vehicles.Where(x=>x.Make.ToLower() == make.ToLower()
            && x.Type.ToLower() == type.ToLower()).ToListAsync(cancellationToken);

            if (allVehicles.Count() == 0)
            {
                return null;
            }

            return allVehicles;
        }


        /// <summary>
        /// Reads all vehicles in DB
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Vehicle>> ReadManyAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.Vehicles.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Read one vehicle by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Vehicle> ReadOneAsync(Guid id, CancellationToken cancellationToken)
        {
            var data = await _appDbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return data;
        }

        /// <summary>
        /// Add many vehicles into DB
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AddManyAsync(List<Vehicle> dataModel, CancellationToken cancellationToken)
        {
            foreach (var dataModelItem in dataModel)
            {
                dataModelItem.CalculateAnnualTaxableLevy(_vehicleOptions);
                dataModelItem.GetRoadworthyTestInterval();
                _appDbContext.Vehicles.Add(dataModelItem);
            }
            _appDbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a vehicle by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOneAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var vehicleToDelete = await _appDbContext.Vehicles.FindAsync(id);

                if (vehicleToDelete == null)
                {
                    _logger.LogInformation($"Specified vechicle not found with ID: {id}");
                    return false;
                }

                _appDbContext.Vehicles.Remove(vehicleToDelete);
                await _appDbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Specified vechicle deleted from DB with ID: {id}");
                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false; 
            }
        }

        /// <summary>
        /// Updates one vechicle information 
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateOneAsync(Vehicle dataModel, CancellationToken cancellationToken)
        {
            try
            {
                var existingVehicle = await _appDbContext.Vehicles.FindAsync(dataModel.Id);

                if (existingVehicle == null)
                {
                    _logger.LogInformation($"Specified vechicle not found with ID: {dataModel.Id}");
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
                existingVehicle.AnnualTaxableLevy = dataModel.AnnualTaxableLevy;
                existingVehicle.RoadworthyTestInterval = dataModel.RoadworthyTestInterval;
                await _appDbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Specified vechicle updated, ID: {dataModel.Id}");
                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false; 
            }
        }

        /// <summary>
        /// Adds one vehicle to DB
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> AddOneAsync(Vehicle dataModel, CancellationToken cancellationToken)
        {
            try
            {
                dataModel.CalculateAnnualTaxableLevy(_vehicleOptions);
                dataModel.GetRoadworthyTestInterval();
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
