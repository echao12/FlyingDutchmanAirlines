using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines_Tests.Stubs;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer{
    public class BookingServiceTests {

        [TestInitialize]
        public async Task TestInitialize(){
        
        }
        [TestMethod]
        public async Task CreateBooking_Success(){
            //BookingRepository bookRepo = new BookingRepository(_context);
            Mock<BookingRepository> mockBookRepo = new Mock<BookingRepository>();
            Mock<CustomerRepository> mockCustRepo = new Mock<CustomerRepository>();

            //sets up the mock to call the fn and expects a completed task.
            //note: moq needs to override the method (thus it has to be virtual)
            //      moq also needs to be able to call a parameterles constructor.
            mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(0, 0)).Returns(Task.CompletedTask);
            mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("Leo Tolstoy"))
                    .Returns(Task.FromResult(new Customer("Leo Tolstoy")));
            
            BookingService service = new BookingService(mockBookRepo.Object, mockCustRepo.Object);//mock.Object to pass the underlying object
            (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 0);
            
            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }
    }
}