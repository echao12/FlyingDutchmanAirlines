using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class AirportRepository {
        private readonly FlyingDutchmanAirlinesContext _context;

        //inject database context
        public AirportRepository(FlyingDutchmanAirlinesContext context) {
            this._context = context;
        }
        public AirportRepository(){
            //ensure only called during tests
            if(Assembly.GetCallingAssembly().FullName == Assembly.GetExecutingAssembly().FullName){
                throw new Exception("This constructor should only be used during testing.");
            }
        }  
        public virtual async Task<Airport> GetAirportById(int airportId){
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