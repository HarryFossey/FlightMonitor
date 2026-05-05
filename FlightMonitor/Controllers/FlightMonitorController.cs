using FlightMonitor.Models;
using FlightMonitor.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightMonitor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightMonitorController(IFlightMonitorService service) : ControllerBase
    {
        [HttpPost($"{nameof(AddFlight)}")]
        public async Task<IActionResult> AddFlight([FromBody] Flight flight)
        {
            await service.AddFlight(flight);
            return CreatedAtAction(nameof(AddFlight), new { origin = flight.Origin, destination = flight.Destination }, flight);
        }

        [HttpPatch("{flightNumber}/price")]
        public async Task<IActionResult> UpdatePrice(string flightNumber, [FromQuery] decimal newPrice)
        {
            await service.UpdatePrice(flightNumber, newPrice);
            return NoContent();
        }

        [HttpGet($"{nameof(GetFlightsBelowThreshold)}")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlightsBelowThreshold([FromQuery] decimal threshold)
        {
            var flights = await service.GetFlightsBelowThreshold(threshold);
            return Ok(flights);
        }
        [HttpGet($"{nameof(GetCheapestFlight)}")]
        public async Task<ActionResult<Flight>> GetCheapestFlight([FromQuery] string origin, [FromQuery] string destination)
        {
            var flight = await service.GetCheapestFlight(origin, destination);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
    }
}
