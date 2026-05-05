using FlightMonitor.Context;
using FlightMonitor.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightMonitor.Tests.Fixtures
{
    public sealed class FlightFixtures : IDisposable
    {
        public FlightFixtures()
        {
            var options = new DbContextOptionsBuilder<FlightContext>()
                .UseInMemoryDatabase($"DBTestInMemory:{Guid.NewGuid()}")
                .Options;

            DbContext = new FlightContext(options);

            DbContext.Flights.AddRange(
                new Flight { FlightNumber = "BA100", Origin = "LHR", Destination = "JFK", Price = 299.99m, DepartureDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Flight { FlightNumber = "BA200", Origin = "LHR", Destination = "CDG", Price = 149.99m, DepartureDate = new DateTime(2026, 6, 2, 0, 0, 0, DateTimeKind.Utc) },
                new Flight { FlightNumber = "BA300", Origin = "MAN", Destination = "JFK", Price = 399.99m, DepartureDate = new DateTime(2026, 6, 3, 0, 0, 0, DateTimeKind.Utc) },
                new Flight { FlightNumber = "BA400", Origin = "MAN", Destination = "JFK", Price = 999.99m, DepartureDate = new DateTime(2026, 6, 3, 0, 0, 0, DateTimeKind.Utc) }
            );
            DbContext.SaveChanges();
        }

        public FlightContext DbContext { get; }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}