using FlightPlanner.Exceptions;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly FlightStorage _storage;

        public CustomerApiController(FlightStorage storage)
        {
            _storage = storage;
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult GetAirport(string search)
        {
            var airport = _storage.GetAirport(search);
            return Ok(airport);
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult SearchFlights(SearchFlightRequest request)
        {
            Flight[] flights;
            try
            {
                flights = _storage.SearchFlights(request);
            }
            catch (EmptyValueException)
            {
                return BadRequest();
            }
            catch (InvalidFlightException)
            {
                return BadRequest();
            }

            return Ok(new PageResult(flights));
        }
        

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult SearchFlightById(int id)
        {
            var flight = _storage.SearchFlightById(id);

            if (flight == null) return NotFound();

            return Ok(flight);
        }
    }
}
