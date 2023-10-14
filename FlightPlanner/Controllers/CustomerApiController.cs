using FlightPlanner.Core.Models;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        //private readonly FlightStorage _storage;

        //public CustomerApiController(FlightStorage storage)
        //{
        //    _storage = storage;
        //}

        [Route("airports")]
        [HttpGet]
        public IActionResult GetAirport(string search)
        {
            Airport airport = null; //_storage.GetAirport(search);
            return Ok(airport);
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult SearchFlights(SearchFlightRequest request)
        {
            Flight[] flights;
            
            flights = Array.Empty<Flight>(); // _storage.SearchFlights(request);
            
            return Ok(new PageResult(flights));
        }
        

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult SearchFlightById(int id)
        {
            Flight flight = null; // _storage.SearchFlightById(id);

            if (flight == null) return NotFound();

            return Ok(flight);
        }
    }
}
