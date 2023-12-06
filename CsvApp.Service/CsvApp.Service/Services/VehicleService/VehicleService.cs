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
        private readonly IDbRepo _dbRepo;

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbRepo"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public VehicleService(IDbRepo dbRepo)
        {
            _dbRepo = dbRepo ?? throw new ArgumentNullException(nameof(dbRepo));
        }

        /// <summary>
        /// returns async list of all vechicles from BD
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Vehicle>> GetAllVehicles(CancellationToken cancellationToken)
        {
            return await _dbRepo.ReadManyAsync(cancellationToken);
        }

        /// <summary>
        /// returns async a specific vehicle from DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Vehicle> GetVehicleById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbRepo.ReadOneAsync(id, cancellationToken);
        }

        /// <summary>
        /// Adds a new vehicle to DB
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Guid> AddVehicle(Vehicle vehicle, CancellationToken cancellationToken)
        {
            var result = await _dbRepo.AddOneAsync(vehicle, cancellationToken);
            if (!result)
            {
                return Guid.Empty;
            }
            return vehicle.Id;
        }

        /// <summary>
        /// Updates a Vechicle information in DB
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateVehicle(Vehicle vehicle, CancellationToken cancellationToken)
        {
            if (vehicle == null)
            {                
                return false;
            }

            return await _dbRepo.UpdateOneAsync(vehicle, cancellationToken); ;
        }


        /// <summary>
        /// Deletes a Vehicle from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteVehicle(Guid id, CancellationToken cancellationToken)
        {
            return await _dbRepo.DeleteOneAsync(id, cancellationToken);

        }

        /// <summary>
        /// Queries vehicles from the DB by make and type
        /// </summary>
        /// <param name="make"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Vehicle>> QueryVehicles(string make, string type, CancellationToken cancellationToken)
        {
            return await _dbRepo.ReadFilterAsync(make,type, cancellationToken);
        }
    }
}
