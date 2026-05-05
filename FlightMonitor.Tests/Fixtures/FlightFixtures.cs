using FlightMonitor.Context;
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
        }

        public FlightContext DbContext { get; }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}