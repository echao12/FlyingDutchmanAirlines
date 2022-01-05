using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines_Tests {
    [TestClass]
    public class FlyingDutchmanAirlinesTests {
        private FlyingDutchmanAirlinesContext _context;
        private AirportRepository _repository;

        [TestInitialize]
        public void TestInitialize() {
            //build a database context
            DbContextOptions<FlyingDutchmanAirlinesContext> dbOptions = 
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;
            this._context = new FlyingDutchmanAirlinesContext(dbOptions); 
            //create new repo with the db context.
            this._repository = new AirportRepository(_context);
            Assert.IsNotNull(_repository);//making sure it was successful
        }
    }
}