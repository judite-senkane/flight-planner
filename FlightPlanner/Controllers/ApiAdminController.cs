using FlightPlanner.Exceptions;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class ApiAdminController : ControllerBase
    {
        private readonly FlightStorage _storage;

        public ApiAdminController()
        {
            _storage = new FlightStorage();
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            return NotFound();
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult AddFlight(Flight flight)
        {
            try
            {
               _storage.AddFlight(flight);
            }
            catch (DuplicateFlightException e)
            {
                return Conflict();
            }

            return Created("", flight);
        }
    }
}
