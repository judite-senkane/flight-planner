using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly FlightStorage _storage;

        public CustomerApiController()
        {
            _storage = new FlightStorage();
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult GetAirport(string search)
        {
            var airport = _storage.GetAirport(search);
            return Ok(airport);
        }
    }
}
