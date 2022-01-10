using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines_Tests.Stubs;
using System;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer {
    [TestClass]
    public class FlightRepositoryTests{
        private FlyingDutchmanAirlinesContext _context;
        private FlightRepository _repository;

        [TestInitialize]
        public void TestInitialize(){
            // establish a db context
            DbContextOptions<FlyingDutchmanAirlinesContext> options = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                                                                            .UseInMemoryDatabase("FlyingDutchman").Options;
            _context = new FlyingDutchmanAirlinesContext_Stub(options);

            //create repository connected to the db context
            this._repository = new FlightRepository(_context);
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
    }
}