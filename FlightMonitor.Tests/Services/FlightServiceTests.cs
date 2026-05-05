using FlightMonitor.Context;
using FlightMonitor.Models;
using FlightMonitor.Services;
using FlightMonitor.Tests.Fixtures;
using Xunit;
using Assert = Xunit.Assert;

namespace FlightMonitor.Tests.Controllers
{
    public sealed class FlightMonitorServiceTests : IDisposable
    {
        private readonly FlightContext _context;
        private readonly FlightMonitorService _sut;
        private readonly FlightFixtures _fixture;

        public FlightMonitorServiceTests()
        {
            _fixture = new FlightFixtures();
            _context = _fixture.DbContext;
            _sut = new FlightMonitorService(_context);
        }
        public void Dispose()
        {
            _fixture.Dispose();
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
            var threshold = 350m;

            // Act
            var results = await _sut.GetFlightsBelowThreshold(threshold);

            // Assert
            Assert.Equal(2, results.Count());
            Assert.All(results, f => Assert.True(f.Price < threshold));
        }

        [Fact]
        public async Task GetCheapestFlight_ReturnsCheapestFlight_WithGivenOriginAndDestination()
        {
            // Act
            var result = await _sut.GetCheapestFlight("MAN", "JFK");
            // Assert
            Assert.NotNull(result);
            Assert.Equal("BA300", result!.FlightNumber);
            Assert.Equal(399.99m, result.Price);
        }
    }
}