using System.Net;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.Controllers{
    public class FlightController : Controller {

        private readonly FlightService _flightService;

        public FlightController(FlightService service){
            _flightService = service;
        }
        public IActionResult GetFlights() {
            return StatusCode((int) HttpStatusCode.OK, "Hello, World!");
        }
    }
}