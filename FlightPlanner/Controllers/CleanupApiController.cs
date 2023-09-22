using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupApiController: ControllerBase
    {
        private readonly FlightStorage _storage = new();

        [Route("clear")]
        [HttpPost]
        public IActionResult ClearFlights()
        {
            _storage.ClearFlights();
            return Ok();
        }
    }
}
