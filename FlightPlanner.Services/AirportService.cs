using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services;

public class AirportService : EntityService<Airport>, IAirportService
{
    public AirportService(IFlightPlannerDbContext context) : base(context)
    {
    }

    public Airport[] FindAirport(string searchPhrase)
    {
        searchPhrase = searchPhrase.Trim().ToLower();

        return _context.Airports.Where(a => 
            a.AirportCode.ToLower().Contains(searchPhrase)||
            a.City.ToLower().Contains(searchPhrase) ||
            a.Country.ToLower().Contains(searchPhrase)).ToArray();
    }
}