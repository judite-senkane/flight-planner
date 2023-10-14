using AutoMapper;
using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers;

[Route("api")]
[ApiController]
public class CustomerApiController : ControllerBase
{
    private readonly IFlightService _flightService;
    private readonly IAirportService _airportService;
    private readonly IMapper _mapper;
    private readonly IEnumerable<IValidateSearch> _validators;

    public CustomerApiController(IFlightService flightService, IAirportService airportService, IMapper mapper, IEnumerable<IValidateSearch> validators)
    {
        _flightService = flightService;
        _airportService = airportService;
        _mapper = mapper;
        _validators = validators;
    }

    [Route("airports")]
    [HttpGet]
    public IActionResult GetAirport(string search)
    {
        var airports = _airportService.FindAirport(search);
        if(airports.Length == 0) return NotFound();

        var result = airports.Select(a => _mapper.Map<AirportRequest>(a)).ToArray();

        return Ok(result);
    }

    [Route("flights/search")]
    [HttpPost]
    public IActionResult SearchFlights(SearchFlightRequest request)
    {
        if (!_validators.All(v => v.IsValid(request))) return BadRequest();

        var flight = MapToFlight(request);

        Flight[] flights = _flightService.SearchFlights(flight);
            
        return Ok(new PageResult(flights));
    }
        

    [Route("flights/{id}")]
    [HttpGet]
    public IActionResult SearchFlightById(int id)
    {
            
        var flight = _flightService.GetFullFlightById(id);

        if (flight == null) return NotFound();

        var result = _mapper.Map<FlightRequest>(flight);

        return Ok(result);
    }

    private Flight MapToFlight(SearchFlightRequest request)
    {
        return new Flight
        {
            DepartureTime = request.DepartureDate,
            From = new Airport { AirportCode = request.From.Trim().ToUpper() },
            To = new Airport { AirportCode = request.To.Trim().ToUpper() }
        };
    }
}