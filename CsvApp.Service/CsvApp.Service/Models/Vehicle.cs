namespace CsvApp.Service.Models
{
    public class Vehicle
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int WheelCount { get; set; }
        public string FuelType { get; set; }
        public bool Active { get; set; }

        // Add methods for calculated fields (Annual Taxable Levy, Roadworthy Test Interval)
       /* public decimal CalculateAnnualTaxableLevy()
        {
            switch (Type)
            {
                case "Car":
                    return FuelType == "Petrol" ? 1.2m * 1500 : 1500;
                case "Bicycle":
                    return 0; // non-applicable
                case "Bike":
                    return 1000;
                case "Plane":
                    return 5000;
                case "Boat":
                    return FuelType == "Petrol" ? 1.15m * 2000 : 2000;
                default:
                    return 0; // Handle other types as needed
            }
        }

        public string GetRoadworthyTestInterval()
        {
            if (Type == "Car")
            {
                return Year < DateTime.Now.Year - 10 ? "Every year" : "Every 2 years";
            }
            else if (Type == "Bike")
            {
                return Year < DateTime.Now.Year - 5 ? "Every 6 months" : "Every year";
            }
            else
            {
                return "No roadworthy test applies";
            }
        }*/
    }
}
