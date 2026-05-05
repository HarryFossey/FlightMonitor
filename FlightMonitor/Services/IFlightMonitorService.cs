using FlightMonitor.Models;

namespace FlightMonitor.Services
{
    public interface IFlightMonitorService
    {
        Task AddFlight(Flight flight);
        Task UpdatePrice(string flightNumber, decimal newPrice);
        Task<IEnumerable<Flight>> GetFlightsBelowThreshold(decimal threshold);
        Task<Flight?> GetCheapestFlight(string origin, string destination);
    }
}
