using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines_Tests.Stubs;

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
    }
}