using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services;

public interface IFlightService : IEntityService<Flight>
{
    Flight? GetFullFlightById(int id);
    Flight?[] SearchFlights(Flight flightSearch);
    bool Exists(Flight flight);
}
