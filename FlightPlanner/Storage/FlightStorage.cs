using FlightPlanner.Exceptions;
using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private readonly FlightPlannerDbContext _context;

        public FlightStorage(FlightPlannerDbContext context)
        {
            _context = context;
        }

        public void AddFlight(Flight flight)
        {
            if (flight.From.AirportCode.ToUpper().Trim() == flight.To.AirportCode.ToUpper().Trim())
            {
                throw new InvalidFlightException();
            }

            if (string.IsNullOrEmpty(flight.From.Country) 
                || string.IsNullOrEmpty(flight.From.City) 
                || string.IsNullOrEmpty(flight.From.AirportCode) 
                || string.IsNullOrEmpty(flight.To.Country) 
                || string.IsNullOrEmpty(flight.To.City) 
                || string.IsNullOrEmpty(flight.To.AirportCode) 
                || string.IsNullOrEmpty(flight.Carrier) 
                || string.IsNullOrEmpty(flight.DepartureTime) 
                || string.IsNullOrEmpty(flight.ArrivalTime))
            {
                throw new EmptyValueException();
            }

            if (_context.Flights.Any(f => f.From.AirportCode == flight.From.AirportCode 
                                          && f.To.AirportCode == flight.To.AirportCode 
                                          && f.DepartureTime == flight.DepartureTime 
                                          && f.ArrivalTime == flight.ArrivalTime))
            {
                throw new DuplicateFlightException();
            }

            if (DateTime.Parse(flight.DepartureTime) >= DateTime.Parse(flight.ArrivalTime))
            {
                throw new InvalidDatesException();
            }

            _context.Flights.Add(flight);
            _context.SaveChanges();
        }

        public void DeleteFlight(int id)
        {
            var flight = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);

            if (flight != null)
            {
                _context.Flights.Remove(flight);
                _context.SaveChanges();
            }
        }

        public void ClearFlights()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
            _context.SaveChanges();
        }

        public Airport[] GetAirport(string searchPhrase)
        {
            searchPhrase = searchPhrase.ToUpper().Trim();
            var airport = _context.Airports
                .Where(a => a.AirportCode.ToUpper().StartsWith(searchPhrase) 
                            || a.Country.ToUpper().StartsWith(searchPhrase) 
                            || a.City.ToUpper().StartsWith(searchPhrase)).ToArray();

            return airport;
        }

        public Flight[] SearchFlights(SearchFlightRequest request)
        {
            if (string.IsNullOrEmpty(request.From) 
                || string.IsNullOrEmpty(request.To) 
                || string.IsNullOrEmpty(request.DepartureDate))
            {
                throw new EmptyValueException();
            }

            if (request.From.Trim().ToUpper() == request.To.Trim().ToUpper())
            {
                throw new InvalidFlightException();
            }

            var airportFrom = GetAirport(request.From);
            var airportTo = GetAirport(request.To);

            if (airportFrom.Length > 0 && airportTo.Length > 0)
            {
                var flights = _context.Flights
                    .Where(f => f.From.AirportCode == airportFrom[0].AirportCode
                                && f.To.AirportCode == airportTo[0].AirportCode
                                && f.DepartureTime.StartsWith(request.DepartureDate)).ToArray();

                return flights;
            }

            return Array.Empty<Flight>();
        }

        public Flight SearchFlightById(int id)
        {
            var flight = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);

            return flight;
        }
    }
}
