﻿using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupApiController: ControllerBase
    {
        private readonly ICleanupService _cleanupService;

        public CleanupApiController(ICleanupService cleanupService)
        {
            _cleanupService = cleanupService;
        }

        [Route("clear")]
        [HttpPost]
        public IActionResult ClearFlights()
        {
            _cleanupService.ClearDatabase();
            return Ok();
        }
    }
}
