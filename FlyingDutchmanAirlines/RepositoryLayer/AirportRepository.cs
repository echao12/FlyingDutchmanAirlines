using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class AirportRepository {
        private readonly FlyingDutchmanAirlinesContext _context;

        //inject database context
        public AirportRepository(FlyingDutchmanAirlinesContext context) {
            this._context = context;
        }
        public async Task<Airport> GetAirportById(int airportId){
            //validate
            if(airportId < 0){
                Console.WriteLine($"Argument Exception in GetAirportById!\nAirportId = {airportId}");
                throw new ArgumentException("Invalid Argument Provided");
            }
            //fetch
            return await _context.Airports.FirstOrDefaultAsync(x => x.AirportId == airportId) ?? throw new AirportNotFoundException();
        }
    }
}