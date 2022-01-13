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
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer{
    public class BookingServiceTests {

        [TestInitialize]
        public async Task TestInitialize(){
        
        }
        [TestMethod]
        public async Task CreateBooking_Success(){
            //create the mocks
            Mock<BookingRepository> mockBookRepo = new Mock<BookingRepository>();
            Mock<CustomerRepository> mockCustRepo = new Mock<CustomerRepository>();

            //use mocks to create a booking and get a customer
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
        [TestMethod]
        [DataRow("", 0)]
        [DataRow(null, 0)]
        [DataRow("alice", -1)]
        [DataRow("jake", 0)]
        [DataRow(null, -1)]
        public async Task CreateBooking_Failure_InvalidInputArguments(string name, int id){
            //setup mock
            Mock<BookingRepository> mockBookRepo = new Mock<BookingRepository>();
            Mock<CustomerRepository> mockCustRepo = new Mock<CustomerRepository>();
            //setup booking service with mocks
            BookingService bookService = new BookingService(mockBookRepo.Object, mockCustRepo.Object);

            (bool result, Exception exception) = await bookService.CreateBooking(name, id);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
        }

        // testing BookingService to handle potential repository exceptions
        // BookingRepository can throw either CouldNotAddBookingToDatbase or ArgumentException Exceptions
        [TestMethod]
        public async Task CreateBooking_Failure_RepositoryException(){
            //create the mocks
            Mock<BookingRepository> mockBookRepo = new Mock<BookingRepository>();
            Mock<CustomerRepository> mockCustRepo = new Mock<CustomerRepository>();

            //setup the mock calls for repository fns to throw exceptions for testing
            mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(0,1)) //for alice cuz her custId = 0. should throw ArgumentException.
                                            .Throws(new ArgumentException());
            mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(1,2)) //for jake to throw this exception
                                            .Throws(new CouldNotAddBookingToDatabaseException());
            
            //setup customer profiles for the mock
            mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("alice")) 
                                                    .Returns(Task.FromResult(new Customer("alice"){ CustomerId = 0}));
            mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("jake"))
                                                    .Returns(Task.FromResult(new Customer("jake"){CustomerId = 1}));
            
            //test CreateBooking Services Exceptions
            BookingService bookService = new BookingService(mockBookRepo.Object, mockCustRepo.Object);
            
            //recall that we constructed the repo mock's to return exceptions as stated above.
            (bool result, Exception exception) = await bookService.CreateBooking("alice", 1);// note: alice's id = 0.
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));

            (result, exception) = await bookService.CreateBooking("jake", 2);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }
    }
}