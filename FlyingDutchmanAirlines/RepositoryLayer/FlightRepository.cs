using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class FlightRepository {
        private FlyingDutchmanAirlinesContext _context;   

        // required for Mock
        [MethodImpl(MethodImplOptions.NoInlining)] //to ensure that this fn will be called with a new stack frame.
        public FlightRepository(){
            //ensure only called during tests
            if(Assembly.GetCallingAssembly().FullName == Assembly.GetExecutingAssembly().FullName){
                throw new Exception("This constructor should only be used during testing.");
            }
        }     
        public FlightRepository( FlyingDutchmanAirlinesContext context ) {
            this._context = context;
        }

        public virtual async Task<Flight> GetFlightByFlightNumber(int flightNumber) {
            if(!flightNumber.IsPositive()) {
                Console.WriteLine($"Could not find flight in GetFlightByFlightNymber. flightNumber = {flightNumber}");
                throw new FlightNotFoundException();
            }
            return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ?? throw new FlightNotFoundException();
        }
    }
}