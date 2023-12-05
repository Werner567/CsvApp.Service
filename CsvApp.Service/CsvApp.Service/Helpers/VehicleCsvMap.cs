using CsvApp.Service.Helpers.Converters;
using CsvApp.Service.Models;
using CsvHelper.Configuration;

namespace CsvApp.Service.Helpers
{
    public class VehicleCsvMap : ClassMap<Vehicle>
    {
        public VehicleCsvMap()
        {
            Map(m => m.Type).Name("TYPE");
            Map(m => m.Make).Name("MAKE");
            Map(m => m.Model).Name("MODEL");
            Map(m => m.Year).Name("YEAR").TypeConverter<CustomInt32Converter>(); ;
            Map(m => m.WheelCount).Name("WHEELCOUNT").TypeConverter<CustomInt32Converter>(); 
            Map(m => m.FuelType).Name("FUELTYPE");
            Map(m => m.Active).Name("ACTIVE").TypeConverter<CustomBooleanConverter>(); ;
        }
    }
}
