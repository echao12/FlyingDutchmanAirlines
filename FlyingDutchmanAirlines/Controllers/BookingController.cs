using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.JsonData;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.Controllers {
    
    [Route("{controller")]
    public class BookingController : Controller {
        BookingService _bookingService;

        public BookingController(BookingService bookingService) {
            this._bookingService = bookingService;
        }

        [HttpPost("{flightNumber}")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingData body, int flightNumber){
            if(ModelState.IsValid && flightNumber.IsPositive()){
                string name = $"{body.FirstName}  {body.LastName}";
                (bool result, Exception exception) = await _bookingService.CreateBooking(name, flightNumber);

                if( result && (exception == null)) {
                    //good to go
                    return StatusCode((int)HttpStatusCode.Created);
                }
                return (exception is CouldNotAddBookingToDatabaseException) ? 
                    StatusCode((int)HttpStatusCode.NotFound) :
                    StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return StatusCode((int) HttpStatusCode.InternalServerError, ModelState.Root.Errors.First().ErrorMessage);
        }
    }
}