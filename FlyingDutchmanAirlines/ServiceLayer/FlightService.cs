using System.Collections.Generic;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines.ServiceLayer{
    public class FlightService{

        private readonly FlightRepository _flightRepo;
        private readonly AirportRepository _airportRepo;
        
        public FlightService(FlightRepository flightRepo, AirportRepository airportRepo){
            this._flightRepo = flightRepo;
            this._airportRepo = airportRepo;
        }

        public async IAsyncEnumerable<FlightView> getFlights(){
            foreach(Flight flight in _flightRepo.GetFlights()){
                //fetch airport info for each flight
                Airport originAirport = await _airportRepo.GetAirportById(flight.Origin);
                Airport destinationAirport = await _airportRepo.GetAirportById(flight.Destination);
                //yield return auto adds the instance to a compiler-generated list.
                yield return new FlightView(flight.FlightNumber.ToString(), 
                    (originAirport.City, originAirport.Iata), (destinationAirport.City, destinationAirport.Iata));
            }
        }
    }
}