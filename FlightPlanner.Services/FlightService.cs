using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services;

public class FlightService : EntityService<Flight>, IFlightService
{
    public FlightService(IFlightPlannerDbContext context) : base(context)
    {
    }

    public Flight? GetFullFlightById(int id)
    {
        return _context.Flights
            .Include(f => f.From)
            .Include(f => f.To)
            .SingleOrDefault(f => f.Id == id);
    }

    public Flight[] SearchFlights(SearchFlightRequest flightSearch)
    {
        var result = _context.Flights.Where(f => f.DepartureTime.StartsWith(flightSearch.DepartureDate) &&
                                    f.From.AirportCode.ToLower() == flightSearch.From.Trim().ToLower() &&
                                    f.To.AirportCode.ToLower() == flightSearch.To.Trim().ToLower()).ToArray();

        return result;
    }

    public bool Exists(Flight flight)
    {
        return _context.Flights.Any(f => f.ArrivalTime == flight.ArrivalTime &&
                                         f.DepartureTime == flight.DepartureTime &&
                                         f.Carrier == flight.Carrier &&
                                         f.From.AirportCode == flight.From.AirportCode &&
                                         f.To.AirportCode == flight.To.AirportCode);
    }
}
