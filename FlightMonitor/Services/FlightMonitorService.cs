using FlightMonitor.Context;
using FlightMonitor.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightMonitor.Services
{
    public class FlightMonitorService(FlightContext context) : IFlightMonitorService
    {
        public async Task AddFlight(Flight flight)
        {
            context.Flights.Add(flight);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePrice(string flightNumber, decimal newPrice)
        {
            var flight = await context.Flights.FindAsync(flightNumber);
            if (flight != null)
            {
                flight.Price = newPrice;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsBelowThreshold(decimal threshold)
        {
            var flights = await context.Flights
                .Where(f => f.Price < threshold)
                .OrderBy(f => f.Price)
                .ToListAsync();

            return flights;
        }

        public async Task<Flight?> GetCheapestFlight(string origin, string destination)
        {
            var flight = await context.Flights
                .Where(f => f.Origin == origin && f.Destination == destination)
                .OrderBy(f => f.Price)
                .FirstOrDefaultAsync();

            return flight;
        }
    }
}
