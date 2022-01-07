using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using System.Threading.Tasks;
using System;
using System.IO;
using FlyingDutchmanAirlines_Tests.Stubs;

namespace FlyingDutchmanAirlines_Tests {
    [TestClass]
    public class FlyingDutchmanAirlinesTests {
        private FlyingDutchmanAirlinesContext _context;
        private AirportRepository _repository;

        [TestInitialize]
        public async Task TestInitialize() {
            //build a database context
            DbContextOptions<FlyingDutchmanAirlinesContext> dbOptions = 
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;
            this._context = new FlyingDutchmanAirlinesContext_Stub(dbOptions); 
            //create a new airport to populate context
            Airport newAirport = new Airport {
                AirportId = 0,
                City = "Nuuk",
                Iata = "GOH"
            };

            //add to the db
            this._context.Airports.Add(newAirport);
            await this._context.SaveChangesAsync();

            //create new repo with the db context.
            this._repository = new AirportRepository(_context);
            Assert.IsNotNull(_repository);//making sure it was successful
        }

        [TestMethod]
        public async Task GetAirportId_Success() {
            Airport airport = await _repository.GetAirportById(0);
            Assert.IsNotNull(airport);
            Assert.AreEqual(0, airport.AirportId);
            Assert.AreEqual("Nuuk", airport.City);
            Assert.AreEqual("GOH", airport.Iata);
        }
        [TestMethod]
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetAirportId_Failure_InvalidInput(int id) {
            StringWriter outputStream = new StringWriter();
            try {    
                Console.SetOut(outputStream);
                await _repository.GetAirportById(id);
            } catch (ArgumentException) {
                Assert.IsTrue(outputStream.ToString().Contains($"Argument Exception in GetAirportById!\nAirportId = {id}"));
                throw new ArgumentException();
            } finally { //note: finally blocks typically executes when control leaves the "try" block. Including exceptions propogating out.
                outputStream.Dispose();
            }
        }
    }
}