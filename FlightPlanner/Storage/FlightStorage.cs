using System.ComponentModel;
using FlightPlanner.Exceptions;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Http.HttpResults;

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
            else if (string.IsNullOrEmpty(flight.From.Country) || string.IsNullOrEmpty(flight.From.City) ||
                string.IsNullOrEmpty(flight.From.AirportCode) || string.IsNullOrEmpty(flight.To.Country) ||
                string.IsNullOrEmpty(flight.To.City) || string.IsNullOrEmpty(flight.To.AirportCode) ||
                string.IsNullOrEmpty(flight.Carrier) || string.IsNullOrEmpty(flight.DepartureTime) ||
                string.IsNullOrEmpty(flight.ArrivalTime))
            {
                throw new EmptyValueException();
            }
            else if (_flightStorage.Any(f => f.From.AirportCode == flight.From.AirportCode && f.To.AirportCode == flight.To.AirportCode && f.DepartureTime == flight.DepartureTime && f.ArrivalTime == flight.ArrivalTime))
            {
                throw new DuplicateFlightException();
            }
            else if (DateTime.Parse(flight.DepartureTime) >= DateTime.Parse(flight.ArrivalTime))
            {
                throw new InvalidDatesException();
            }
            flight.Id = _id++;
            _flightStorage.Add(flight);
        }

        public void ClearFlights()
        {
            _flightStorage.Clear();
        }
    }
}
