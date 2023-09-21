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
            if (_flightStorage.Any(f => f.From == flight.From && f.To == flight.To && f.DepartureTime == flight.DepartureTime && f.ArrivalTime == flight.ArrivalTime))
            {
                throw new DuplicateFlightException();
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
