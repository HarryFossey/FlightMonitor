namespace FlightMonitor.Models
{
    public class Flight
    {
        public required string FlightNumber { get; set; }
        public required string Origin { get; set; }
        public required string Destination { get; set; }
        public decimal Price { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
