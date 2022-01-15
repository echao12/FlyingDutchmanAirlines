using System.Collections.Generic;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
namespace FlyingDutchmanAirlines.ServiceLayer{
    public class FlightService{

        public readonly FlightRepository _flightRepo;
        
        public FlightService(FlightRepository flightRepo){
            this._flightRepo = flightRepo;
        }
    }
}