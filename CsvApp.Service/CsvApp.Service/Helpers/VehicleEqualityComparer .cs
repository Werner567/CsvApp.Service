using CsvApp.Service.Models;

namespace CsvApp.Service.Helpers
{
    public class VehicleEqualityComparer : IEqualityComparer<Vehicle>
    {
        public bool Equals(Vehicle x, Vehicle y)
        {
            // Implement your own logic for equality comparison
            return x.Make == y.Make && x.Model == y.Model;
        }

        public int GetHashCode(Vehicle obj)
        {
            // Implement your own logic for generating a hash code
            return $"{obj.Make}_{obj.Model}".GetHashCode();
        }
    }
}
