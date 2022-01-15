using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines_Tests.Stubs;
using System;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using System.Collections.Generic;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer {
    [TestClass]
    public class FlightRepositoryTests{
        private FlyingDutchmanAirlinesContext _context;
        private FlightRepository _repository;

        [TestInitialize]
        public async Task TestInitialize(){
            // establish a db context
            DbContextOptions<FlyingDutchmanAirlinesContext> options = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                                                                            .UseInMemoryDatabase("FlyingDutchman").Options;
            _context = new FlyingDutchmanAirlinesContext_Stub(options);

            //add a flight entry to the database
            Flight flight = new Flight{
                FlightNumber = 1,
                Origin = 1,
                Destination = 2
            };

            Flight flight2 = new Flight{
                FlightNumber = 10,
                Origin = 3,
                Destination = 4
            };

            _context.Flights.Add(flight);
            _context.Flights.Add(flight2);
            await _context.SaveChangesAsync();

            //create repository connected to the db context
            this._repository = new FlightRepository(_context);
            Assert.IsNotNull(this._repository);
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Success() {
            //grab a flight from the repo
            Flight flight = await _repository.GetFlightByFlightNumber(1);
            Assert.IsNotNull(flight);
            //query database for the same flight
            Flight dbFlight = await _context.Flights.FirstAsync(f => f.FlightNumber == 1);
            Assert.IsNotNull(dbFlight);

            Assert.AreEqual(dbFlight.FlightNumber, flight.FlightNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber(){
            await _repository.GetFlightByFlightNumber(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByFlightNumber_Faulure_DatabaseException(){
            await _repository.GetFlightByFlightNumber(2);
        }

        [TestMethod]
        public void GetFlights_Success(){
            Queue<Flight> flights = _repository.GetFlights();
            Assert.IsNotNull(flights);
        }
    }
}