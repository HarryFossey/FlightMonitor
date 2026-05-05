using FlightMonitor.Context;
using FlightMonitor.Models;
using FlightMonitor.Services;
using FlightMonitor.Tests.Fixtures;
using Xunit;
using Assert = Xunit.Assert;

namespace FlightMonitor.Tests.Controllers
{
    public class FlightMonitorServiceTests : IClassFixture<FlightFixtures>
    {
        private readonly FlightContext _context;
        private readonly FlightMonitorService _sut;

        public FlightMonitorServiceTests(FlightFixtures fixture)
        {
            _context = fixture.DbContext;
            _sut = new FlightMonitorService(_context);
        }

        [Fact]
        public async Task AddFlight_WhenCalled_AddsFlight()
        {
            // Arrange
            var flight = new Flight
            {
                FlightNumber = "Flight1",
                Origin = "LDN",
                Destination = "NYC",
                Price = 199.99m
            };

            // Act
            await _sut.AddFlight(flight);

            // Assert
            var addedFlight = await _context.Flights.FindAsync("Flight1");
            Assert.Equal("LDN", addedFlight!.Origin);
            Assert.Equal("NYC", addedFlight.Destination);
            Assert.Equal(199.99m, addedFlight.Price);
        }

        [Fact]
        public async Task UpdatePrice_WhenCalled_UpdatesFlightPrice()
        {
            // Arrange
            var flight = new Flight
            {
                FlightNumber = "Flight2",
                Origin = "LDN",
                Destination = "NYC",
                Price = 199.99m
            };
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            // Act
            await _sut.UpdatePrice("Flight2", 149.99m);
            // Assert
            var updatedFlight = await _context.Flights.FindAsync("Flight2");
            Assert.Equal(149.99m, updatedFlight!.Price);
        }

        [Fact]
        public async Task GetFlightsBelowThreshold_ReturnsCorrectFlights_BelowThreshold()
        {
            //Arrange
            var threshold = 300m;
            await SeedFlights();

            // Act
            var results = await _sut.GetFlightsBelowThreshold(threshold);

            // Assert
            Assert.Equal(2, results.Count());
            Assert.All(results, f => Assert.True(f.Price < threshold));
        }

        [Fact]
        public async Task GetCheapestFlight_ReturnsCheapestFlight_WithGivenOriginAndDestination()
        {
            // Arrange
            await SeedFlights();

            // Act
            var result = await _sut.GetCheapestFlight("MAN", "JFK");
            // Assert
            Assert.NotNull(result);
            Assert.Equal("BA300", result!.FlightNumber);
            Assert.Equal(399.99m, result.Price);
        }

        private async Task SeedFlights()
        {
            var flights = new List<Flight>
            {
                new() { FlightNumber = "BA100", Origin = "LHR", Destination = "JFK", Price = 299.99m, DepartureDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
                new() { FlightNumber = "BA200", Origin = "LHR", Destination = "CDG", Price = 149.99m, DepartureDate = new DateTime(2026, 6, 2, 0, 0, 0, DateTimeKind.Utc) },
                new() { FlightNumber = "BA300", Origin = "MAN", Destination = "JFK", Price = 399.99m, DepartureDate = new DateTime(2026, 6, 3, 0, 0, 0, DateTimeKind.Utc) },
                new() { FlightNumber = "BA400", Origin = "MAN", Destination = "JFK", Price = 999.99m, DepartureDate = new DateTime(2026, 6, 3, 0, 0, 0, DateTimeKind.Utc) }
            };

            await _context.Flights.AddRangeAsync(flights);
            await _context.SaveChangesAsync();
        }
    }
}