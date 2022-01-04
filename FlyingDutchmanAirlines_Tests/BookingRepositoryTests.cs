using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines_Tests.Stubs;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer {
    [TestClass]
    public class BookingRepositoryTests {
        private FlyingDutchmanAirlinesContext _context;
        private BookingRepository _repository;

        [TestInitialize] 
        public void TestInitialize() {
            //build the database context with the options from the FlyingDutchmanContext
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = 
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                    .UseInMemoryDatabase("FlyingDutchman").Options;
            //create new FlyingDutchmanAirlinesContext
            this._context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);
            //create a new repository with the db context.
            this._repository = new BookingRepository(_context);
            Assert.IsNotNull(_repository);
        }

        [TestMethod]
        public void CreateBooking_Success() {

        }

        [TestMethod]
        [DataRow(-1, 0)]
        [DataRow(0, -1)]
        [DataRow(-1, -1)]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateBooking_Failure_InvalidInputs(int customerId, int flightNumber){
            await _repository.CreateBooking(customerId, flightNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(CouldNotAddBookingToDatabaseException))]
        public async Task CreateBooking_Failure_DatabaseError() {
            await _repository.CreateBooking(0, 1);
        }
    }
}