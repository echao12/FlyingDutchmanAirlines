using FlyingDutchmanAirlines.DatabaseLayer;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class AirportRepository {
        private readonly FlyingDutchmanAirlinesContext _context;

        //inject database context
        public AirportRepository(FlyingDutchmanAirlinesContext context) {
            this._context = context;
        }
        
    }
}