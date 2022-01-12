using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines_Tests.Stubs;
using Moq;

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
            BookingRepository bookRepo = new BookingRepository(_context);
            Mock<BookingRepository> mockRepo = new Mock<BookingRepository>();
            //sets up the mock to call the fn and expects a completed task.
            //note: moq needs to override the method (thus it has to be virtual)
            //      moq also needs to be able to call a parameterles constructor.
            mockRepo.Setup(bookRepo => bookRepo.CreateBooking(0, 0)).Returns(Task.CompletedTask);
            BookingService service = new BookingService(mockRepo.Object);//mock.Object to pass the underlying object
            (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 0);
            
            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }
    }
}