using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using System;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class FlightRepository {
        private FlyingDutchmanAirlinesContext _context;        
        public FlightRepository( FlyingDutchmanAirlinesContext context ) {
            this._context = context;
        }

        public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int originAriportId, int destinationAirportId) {
            if(!flightNumber.IsPositive() || !originAriportId.IsPositive()){
                Console.WriteLine("Argument exception in GetFlightByFlightNumber.\n" 
                    + $"originAirportId = {originAriportId} destinationAirportId = {destinationAirportId}");
                throw new ArgumentException("Invalid arguments were provided!");
            }
            if(!flightNumber.IsPositive()) {
                Console.WriteLine($"Could not find flight in GetFlightByFlightNymber. flightNumber = {flightNumber}");
                throw new FlightNotFoundException();
            }
            return new Flight();
        }
    }
}