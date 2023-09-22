using FlightPlanner.Exceptions;
using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flightStorage = new ();
        private static int _id = 0;

        public void AddFlight(Flight flight)
        {
            if (flight.From.AirportCode.ToUpper().Trim() == flight.To.AirportCode.ToUpper().Trim())
            {
                throw new InvalidFlightException();
            }

            if (string.IsNullOrEmpty(flight.From.Country) || string.IsNullOrEmpty(flight.From.City) ||
                string.IsNullOrEmpty(flight.From.AirportCode) || string.IsNullOrEmpty(flight.To.Country) ||
                string.IsNullOrEmpty(flight.To.City) || string.IsNullOrEmpty(flight.To.AirportCode) ||
                string.IsNullOrEmpty(flight.Carrier) || string.IsNullOrEmpty(flight.DepartureTime) ||
                string.IsNullOrEmpty(flight.ArrivalTime))
            {
                throw new EmptyValueException();
            }

            if (_flightStorage.Any(f => f.From.AirportCode == flight.From.AirportCode && f.To.AirportCode == flight.To.AirportCode && f.DepartureTime == flight.DepartureTime && f.ArrivalTime == flight.ArrivalTime))
            {
                throw new DuplicateFlightException();
            }

            if (DateTime.Parse(flight.DepartureTime) >= DateTime.Parse(flight.ArrivalTime))
            {
                throw new InvalidDatesException();
            }

            flight.Id = _id++;
            _flightStorage.Add(flight);
        }

        public void DeleteFlight(int id)
        {
            var flight = _flightStorage.FirstOrDefault(f => f.Id == id);
            _flightStorage.Remove(flight);
        }

        public void ClearFlights()
        {
            _flightStorage.Clear();
        }

        public Airport[] GetAirport(string searchPhrase)
        {
            searchPhrase = searchPhrase.ToUpper().Trim();
            var airport = _flightStorage
                .SelectMany(f => new[] { f.From, f.To }).ToHashSet()
                .Where(a => a.AirportCode.ToUpper().StartsWith(searchPhrase) || 
                    a.Country.ToUpper().StartsWith(searchPhrase) || a.City.ToUpper()
                        .StartsWith(searchPhrase)).ToArray();

            return airport;
        }

        public Flight[] SearchFlights(SearchFlightRequest request)
        {
            if (string.IsNullOrEmpty(request.From) || string.IsNullOrEmpty(request.To) ||
                string.IsNullOrEmpty(request.DepartureDate))
            {
                throw new EmptyValueException();
            }

            if (request.From.Trim().ToUpper() == request.To.Trim().ToUpper())
            {
                throw new InvalidFlightException();
            }

            var airportFrom = GetAirport(request.From);
            var airportTo = GetAirport(request.To);
            var flights = _flightStorage.Where(f => f.From.AirportCode == airportFrom[0].AirportCode 
                                                            && f.To.AirportCode == airportTo[0].AirportCode 
                                                            && f.DepartureTime.StartsWith(request.DepartureDate)).ToArray();

            return flights;
        }

        public Flight SearchFlightById(int id)
        {
            var flight = _flightStorage.FirstOrDefault(f => f.Id == id);
            return flight;
        }
    }
}
