using Lot.FlightManagementSystem.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace Lot.FlightManagementSystem.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasKey(f => f.FlightId);
        }
    }
}
