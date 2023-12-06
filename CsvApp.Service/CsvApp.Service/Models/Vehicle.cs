using CsvApp.Service.Options;
using Microsoft.Extensions.Options;

namespace CsvApp.Service.Models
{
    public class Vehicle
    {
       
        public Guid Id { get; set; } = Guid.NewGuid(); //Creates new ID for DB write
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int WheelCount { get; set; }
        public string FuelType { get; set; }
        public bool Active { get; set; }

        public decimal AnnualTaxableLevy { get; set; }
        public string RoadworthyTestInterval { get; set; }

        
        public void CalculateAnnualTaxableLevy(IOptions<VehicleOptions> options)
        {
            switch (Type.ToLower())
            {
                case "car":
                    AnnualTaxableLevy = FuelType.ToLower() == "petrol" ? options.Value.CarLevy * (1 + options.Value.PetrolLevyPercentage) : options.Value.CarLevy;
                    break;
                case "bicycle":
                    AnnualTaxableLevy = options.Value.BicycleLevy; 
                    break;
                case "bike":
                    AnnualTaxableLevy = options.Value.BikeLevy;
                    break;
                case "plane":
                    AnnualTaxableLevy = options.Value.PlaneLevy;
                    break;
                case "boat":
                    AnnualTaxableLevy = FuelType.ToLower() == "petrol" ? options.Value.BoatLevy * (1+ options.Value.BoatLevyPercentage) : 2000;
                    break;
                default:
                    AnnualTaxableLevy = 0;
                    break;
            }
        }


        public void GetRoadworthyTestInterval()
        {
            if (Type == "Car")
            {
                RoadworthyTestInterval =  Year < DateTime.Now.Year - 10 ? "Every year" : "Every 2 years";
             }
            else if (Type == "Bike")
            {
                RoadworthyTestInterval = Year < DateTime.Now.Year - 5 ? "Every 6 months" : "Every year";
            }
            else
            {
                RoadworthyTestInterval = "No roadworthy test applies";
            }
        }
    }
}
