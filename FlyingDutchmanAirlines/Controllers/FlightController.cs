using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.Controllers{
    public class FlightController : Controller {

        private readonly FlightService _flightService;

        public FlightController(FlightService service){
            _flightService = service;
        }
        public async Task<IActionResult> GetFlights() {
            try{
                Queue<FlightView> flightViews = new Queue<FlightView>();
                await foreach(FlightView view in _flightService.GetFlights()){
                    flightViews.Enqueue(view);
                }
                return StatusCode((int)HttpStatusCode.OK, flightViews);
            }catch(FlightNotFoundException){
                return StatusCode((int) HttpStatusCode.NotFound, "No flights were found in the database");
            }catch(Exception){
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
            }
        }
    }
}