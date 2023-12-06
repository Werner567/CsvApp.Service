using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;
using CsvApp.Service.Helpers;
using CsvApp.Service.Models;
using CsvApp.Service.Repos;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CsvApp.Service.Services.CsvLoaderService
{
    public class CsvLoaderService : ICsvLoaderService
    {
        private List<Vehicle> _loadedData;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CsvLoaderService> _logger;
        private readonly IDbRepo _options;

        //private readonly AppDbContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CsvLoaderService(IServiceProvider serviceProvider, ILogger<CsvLoaderService> logger, IDbRepo options)
        {

            // Initialize other members...
            _loadedData = new List<Vehicle>();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            //_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

       
        

        /// <summary>
        /// Reading the CSV file
        /// </summary>
        /// <param name="filePath">full path</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LoadCsv(string filePath, CancellationToken cancellationToken)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                _logger.LogInformation("No CSV file found, skipping data import");
            }

            if (File.Exists(filePath))
            {
                _logger.LogInformation($"attempting to read {filePath}, importing new data not in DB");

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null }))
                {
                    _logger.LogTrace($"Attempting Registing Classmap");
                    csv.Context.RegisterClassMap<VehicleCsvMap>();
                    csv.Read();
                    _logger.LogTrace($"Reading file");
                    csv.ReadHeader();

                    // Get records without using HasHeaderRecord configuration
                    _loadedData = csv.GetRecords<Vehicle>().ToList();


                    var dataInDb = await _options.ReadFilterManyAsync(_loadedData, cancellationToken);
                    var dataToAdd = CompareLists(_loadedData, dataInDb);
                    // Save new records found to the database
                    _logger.LogInformation($"adding {dataToAdd.Count} new recods to the DB");
                    await _options.AddManyAsync(dataToAdd, cancellationToken);

                }
            }
        }





        private List<Vehicle> CompareLists(List<Vehicle> list1, List<Vehicle> list2)
        {
            if (list2 == null)
            {
                return list1;
            }
            // Find vehicles in list1 that are not in list2
            var addedVehicles = list1.Except(list2, new VehicleEqualityComparer()).ToList();

            // Find vehicles in list2 that are not in list1
            var removedVehicles = list2.Except(list1, new VehicleEqualityComparer()).ToList();

            // Combine the added and removed vehicles
            var differences = addedVehicles.Concat(removedVehicles).ToList();

            return differences;
        }
    }
}
