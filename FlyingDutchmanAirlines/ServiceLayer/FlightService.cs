using System.Collections.Generic;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;
using System.Threading.Tasks;
using System;
using FlyingDutchmanAirlines.Exceptions;
using System.Reflection;

namespace FlyingDutchmanAirlines.ServiceLayer{
    public class FlightService{

        private readonly FlightRepository _flightRepo;
        private readonly AirportRepository _airportRepo;
        
        public FlightService(FlightRepository flightRepo, AirportRepository airportRepo){
            this._flightRepo = flightRepo;
            this._airportRepo = airportRepo;
        }

        public FlightService(){
            if(Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName){
                throw new Exception("This constructor should only be used for testing");
            }
        }

        public virtual async IAsyncEnumerable<FlightView> GetFlights(){
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

        public virtual async Task<FlightView> GetFlightByFlightNumber(int flightNumber){
            try {
                Flight flight = await _flightRepo.GetFlightByFlightNumber(flightNumber);
                Airport origin = await _airportRepo.GetAirportById(flight.Origin);
                Airport Destination = await _airportRepo.GetAirportById(flight.Destination);

                FlightView view = new FlightView(flightNumber.ToString(), (origin.City, origin.Iata), (Destination.City, Destination.Iata));
                return view;
            }catch(FlightNotFoundException){
                throw new FlightNotFoundException();
            }catch(Exception){
                throw new ArgumentException();
            }
        }
    }
}