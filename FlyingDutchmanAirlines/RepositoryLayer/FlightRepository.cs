using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class FlightRepository {
        private FlyingDutchmanAirlinesContext _context;        
        public FlightRepository( FlyingDutchmanAirlinesContext context ) {
            this._context = context;
        }

        public Flight GetFlightByFlightNumber(int flightNumber) {
            return new Flight();
        }
    }
}