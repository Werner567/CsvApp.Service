using Microsoft.EntityFrameworkCore;

namespace CsvApp.Service.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }   
}
