using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines_Tests.Stubs;
using System;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

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
            _context.Flights.Add(flight);

            await _context.SaveChangesAsync();
            //create repository connected to the db context
            this._repository = new FlightRepository(_context);
            Assert.IsNotNull(this._repository);
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Success() {
            //grab a flight from the repo
            Flight flight = await _repository.GetFlightByFlightNumber(1, 1, 2);
            Assert.IsNotNull(flight);
            //query database for the same flight
            Flight dbFlight = await _context.Flights.FirstAsync(f => f.FlightNumber == 1);
            Assert.IsNotNull(dbFlight);

            Assert.AreEqual(dbFlight.FlightNumber, flight.FlightNumber);
            Assert.AreEqual(dbFlight.Origin, flight.Origin);
            Assert.AreEqual(dbFlight.Destination, flight.Destination);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFlightByFlightNumber_Failure_InvalidOriginAirportId() {
            await _repository.GetFlightByFlightNumber(0, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFlightByFlightNumber_Failure_InvalidDestinationAirportId(){
            await _repository.GetFlightByFlightNumber(0, 0, -1);
        }
        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber(){
            await _repository.GetFlightByFlightNumber(-1, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByFlightNumber_Faulure_DatabaseException(){
            await _repository.GetFlightByFlightNumber(2, 1, 2);
        }
    }
}