using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines_Tests.Stubs;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer{
    public class BookingServiceTests {
        private FlyingDutchmanAirlinesContext _context;

        [TestInitialize]
        public async Task TestInitialize(){
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = 
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                    .UseInMemoryDatabase("FlyingDutchman").Options;
            _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);
            
        }
        [TestMethod]
        public async Task CreateBooking_Success(){
            BookingRepository bRepository = new BookingRepository(_context);
            //BookingService bService = new BookingService(bRepository);
        }
    }
}