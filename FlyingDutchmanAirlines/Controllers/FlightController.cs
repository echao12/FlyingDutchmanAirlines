using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.Controllers{
    [Route("{controller}")]
    public class FlightController : Controller {

        private readonly FlightService _flightService;

        public FlightController(FlightService service){
            _flightService = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpGet("{flightNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber){
            try{
                if(!flightNumber.IsPositive()){
                    throw new Exception();
                }
                FlightView view = await _flightService.GetFlightByFlightNumber(flightNumber);
                return StatusCode((int)HttpStatusCode.OK, view);
            }catch(FlightNotFoundException){
                return StatusCode((int)HttpStatusCode.NotFound, "The flight was not found in the database");
            }catch(Exception){
                return StatusCode((int)HttpStatusCode.BadRequest, "Bad request");
            }
        }
    }
}