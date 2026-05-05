using FlightMonitor.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightMonitor.Context
{
    public class FlightContext(DbContextOptions<FlightContext> options) : DbContext(options)
    {
        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Using FlightNumber as PK, this assumes that each flight number is unique across all flights, otherwise would add new FlightId Key.
            modelBuilder.Entity<Flight>().HasKey(f => f.FlightNumber);
        }
    }
}
