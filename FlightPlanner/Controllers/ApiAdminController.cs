using AutoMapper;
using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers;

[Authorize]
[Route("admin-api")]
[ApiController]
public class ApiAdminController : ControllerBase
{
    private readonly IFlightService _flightService;
    private readonly IMapper _mapper;
    private readonly IEnumerable<IValidate> _validators;
    private static readonly object _controllerLock = new();

    public ApiAdminController(IFlightService flightService, IMapper mapper, IEnumerable<IValidate> validators)
    {
        _flightService = flightService;
        _mapper = mapper;
        _validators = validators;
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

        lock (_controllerLock)
        {
            if (!_validators.All(v => v.IsValid(flight)))
            {
                return BadRequest();
            }

            if (_flightService.Exists(flight))
            {
                return Conflict();
            }

            _flightService.Create(flight);
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
            if (flight != null) _flightService.Delete(flight);

            return Ok();
        }
    }
}