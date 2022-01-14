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
    [TestClass]
    public class BookingServiceTests {

        private Mock<BookingRepository> _mockBookRepo;
        private Mock<CustomerRepository> _mockCustRepo;
        private Mock<FlightRepository> _mockFlightRepo;
        private BookingService _bookService;

        [TestInitialize]
        public void TestInitialize(){
            _mockBookRepo = new Mock<BookingRepository>();
            _mockCustRepo = new Mock<CustomerRepository>();
            _mockFlightRepo = new Mock<FlightRepository>();
            _bookService = new BookingService(_mockBookRepo.Object, _mockCustRepo.Object, _mockFlightRepo.Object);
        }
        [TestMethod]
        public async Task CreateBooking_Success(){
            //use mocks to create a booking and get a customer
            //note: moq needs to override the method (thus it has to be virtual)
            //      moq also needs to be able to call a parameterles constructor.
            _mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(0, 0)).Returns(Task.CompletedTask);
            _mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("Leo Tolstoy"))
                    .Returns(Task.FromResult(new Customer("Leo Tolstoy")));
            
            (bool result, Exception exception) = await _bookService.CreateBooking("Leo Tolstoy", 0);
            
            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }

        [TestMethod]
        [DataRow("", 0)]
        [DataRow(null, 0)]
        [DataRow("alice", -1)]
        [DataRow("", -1)]
        [DataRow(null, -1)]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateBooking_Failure_InvalidInputArguments(string name, int id){
            (bool result, Exception exception) = await _bookService.CreateBooking(name, id);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
        }

        // *** Testing BookingService to handle potential repository exceptions ***
        // *** BookingRepository can throw either CouldNotAddBookingToDatbase or ArgumentException Exceptions ***

        //Case when repo layer throws ArgumentException to the service layer
        [TestMethod]
        public async Task CreateBooking_Failure_ArgumentException(){
            //setup the mock calls for repository fns to throw exceptions for testing
            _mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(0,1)) //for alice cuz her custId = 0. should throw ArgumentException.
                                            .Throws(new ArgumentException());
            
            //setup customer profiles for the mock
            _mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("alice")) 
                                                    .Returns(Task.FromResult(new Customer("alice"){ CustomerId = 0}));
            //recall that we constructed the repo mock's to return exceptions as stated above.
            (bool result, Exception exception) = await _bookService.CreateBooking("alice", 1);// note: alice's id = 0.
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
        }

        //Case when repo layer throws CouldNotAddBookingToDatabaseException to the service layer
        [TestMethod]
        public async Task CreateBooking_Failure_CouldNotAddBookingToDatabase(){
            _mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(1,2))
                                                    .Throws(new CouldNotAddBookingToDatabaseException());
            _mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("jake"))
                                                    .Returns(Task.FromResult(new Customer("jake"){CustomerId = 1}));
            
            (bool result, Exception exception) = await _bookService.CreateBooking("jake", 2);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }
    }
}