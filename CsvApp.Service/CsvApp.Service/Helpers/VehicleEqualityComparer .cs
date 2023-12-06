using CsvApp.Service.Models;

namespace CsvApp.Service.Helpers
{
    public class VehicleEqualityComparer : IEqualityComparer<Vehicle>
    {
        public bool Equals(Vehicle x, Vehicle y)
        {
            return x.Make == y.Make && x.Model == y.Model;
        }

        public int GetHashCode(Vehicle obj)
        {
            return $"{obj.Make}_{obj.Model}".GetHashCode();
        }
    }
}
