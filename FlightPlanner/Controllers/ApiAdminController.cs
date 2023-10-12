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
        private static readonly object _controllerLock = new();

        public ApiAdminController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetFullFlightById(id);

            if (flight == null) return NotFound();

            return Ok(flight);
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult AddFlight(FlightRequest request)
        {
            var flight = MapToFlight(request);
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

            request = MapToFlightRequest(flight);

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

        private Flight MapToFlight (FlightRequest request)
        {
            return new Flight
            {
                Id = request.Id,
                ArrivalTime = request.ArrivalTime,
                Carrier = request.Carrier,
                DepartureTime = request.DepartureTime,
                From = new Airport
                {
                    City = request.From.City,
                    Country = request.From.Country,
                    AirportCode = request.From.Airport
                },
                To = new Airport
                {
                    City = request.To.City,
                    Country = request.To.Country,
                    AirportCode = request.To.Airport
                }
            };
        }
        
        private FlightRequest MapToFlightRequest (Flight flight)
        {
            return new FlightRequest
            {
                Id = flight.Id,
                ArrivalTime = flight.ArrivalTime,
                Carrier = flight.Carrier,
                DepartureTime = flight.DepartureTime,
                From = new AirportRequest
                {
                    City = flight.From.City,
                    Country = flight.From.Country,
                    Airport = flight.From.AirportCode
                },
                To = new AirportRequest 
                {
                   City = flight.To.City,
                   Country = flight.To.Country,
                   Airport = flight.To.AirportCode
                }
            };
        }
    }
}
