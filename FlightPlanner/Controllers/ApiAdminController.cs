using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Exceptions;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class ApiAdminController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private static readonly object _controllerLock = new();

        public ApiAdminController(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetFullFlightById(id);

            if (flight == null) return NotFound();

            return Ok(_mapper.Map<FlightRequest>(flight));
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult AddFlight(FlightRequest request)
        {
            var flight = _mapper.Map<Flight>(request);
            try
            {
                lock (_controllerLock)
                {
                    _flightService.Create(flight);
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

            request = _mapper.Map<FlightRequest>(flight);

            return Created("", request);
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (_controllerLock)
            {
                var flight = _flightService.GetFullFlightById(id);
                _flightService.Delete(flight);

                return Ok();
            }
        }
    }
}
