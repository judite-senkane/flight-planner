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
        private static readonly object _controllerLock = new();

        public ApiAdminController(FlightStorage storage)
        {
            _storage = storage;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = _storage.SearchFlightById(id);

            if (flight == null) return NotFound();

            return Ok(flight);
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult AddFlight(Flight flight)
        {
            try
            {
                lock (_controllerLock)
                {
                   _storage.AddFlight(flight);
                }
            }
            catch (InvalidFlightException)
            {
                return BadRequest();
            }
            catch (EmptyValueException)
            {
                return BadRequest();
            }
            catch (InvalidDatesException)
            {
                return BadRequest();
            }
            catch (DuplicateFlightException)
            {
                return Conflict();
            }

            return Created("", flight);
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (_controllerLock)
            {
                _storage.DeleteFlight(id);

                return Ok();
            }
        }
    }
}
