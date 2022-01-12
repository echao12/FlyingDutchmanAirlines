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
using System.Collections.Generic;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer {
    [TestClass]
    public class AirportRepositoryTests {
        private FlyingDutchmanAirlinesContext _context;
        private AirportRepository _repository;

        [TestInitialize]
        public async Task TestInitialize() {
            //build a database context
            DbContextOptions<FlyingDutchmanAirlinesContext> dbOptions = 
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;
            this._context = new FlyingDutchmanAirlinesContext_Stub(dbOptions); 
            //create a new airport to populate context
            SortedList<string, Airport> newAirports = new SortedList<string, Airport> {
                {
                    "GOH",
                    new Airport
                    {
                    AirportId = 0,
                    City = "Nuuk",
                    Iata = "GOH"
                    }
                },
                {
                    "PHX",
                    new Airport
                    {
                    AirportId = 1,
                    City = "Phoenix",
                    Iata = "PHX"
                    }
                },
                {
                    "DDH",
                    new Airport
                    {
                    AirportId = 2,
                    City = "Bennington",
                    Iata = "DDH"
                    }
                },
                {
                    "RDU",
                    new Airport
                    {
                    AirportId = 3,
                    City = "Raleigh-Durham",
                    Iata = "RDU"
                    }
                }
            };

            //add to the db
            this._context.Airports.AddRange(newAirports.Values);
            await this._context.SaveChangesAsync();

            //create new repo with the db context.
            this._repository = new AirportRepository(_context);
            Assert.IsNotNull(_repository);//making sure it was successful
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetAirportId_Success(int id) {
            Airport airport = await _repository.GetAirportById(id);
            Assert.IsNotNull(airport);
            Airport dbEntryAirport = await this._context.Airports.FirstAsync( a => a.AirportId == id);
            Assert.AreEqual(dbEntryAirport.AirportId, airport.AirportId);
            Assert.AreEqual(dbEntryAirport.City, airport.City);
            Assert.AreEqual(dbEntryAirport.Iata, airport.Iata);
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
        [TestMethod]
        [ExpectedException(typeof(AirportNotFoundException))]
        public async Task GetAirportById_Failure_DatabaseException() {
            await _repository.GetAirportById(10);
        }
    }
}