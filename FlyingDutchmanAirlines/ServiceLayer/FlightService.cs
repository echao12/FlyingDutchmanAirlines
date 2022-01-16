using System.Collections.Generic;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;
using System.Threading.Tasks;
using System;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.ServiceLayer{
    public class FlightService{

        private readonly FlightRepository _flightRepo;
        private readonly AirportRepository _airportRepo;
        
        public FlightService(FlightRepository flightRepo, AirportRepository airportRepo){
            this._flightRepo = flightRepo;
            this._airportRepo = airportRepo;
        }

        public async IAsyncEnumerable<FlightView> GetFlights(){
            foreach(Flight flight in _flightRepo.GetFlights()){
                //fetch origin/destination locations info for each flight
                Airport originAirport;
                Airport destinationAirport;
                try{
                    originAirport = await _airportRepo.GetAirportById(flight.Origin);
                    destinationAirport = await _airportRepo.GetAirportById(flight.Destination);
                }catch(FlightNotFoundException){
                    throw new FlightNotFoundException();
                }catch(Exception){
                    throw new ArgumentException();
                }
                //yield return auto adds the instance to a compiler-generated list.
                yield return new FlightView(flight.FlightNumber.ToString(), 
                    (originAirport.City, originAirport.Iata), (destinationAirport.City, destinationAirport.Iata));
            }
        }
    }
}