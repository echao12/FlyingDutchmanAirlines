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
            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlightByFlightNumber(0))
                    .ReturnsAsync(new Flight());
            
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
        public async Task CreateBooking_Failure_InvalidInputArguments(string name, int id){
            (bool result, Exception exception) = await _bookService.CreateBooking(name, id);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
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
            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlightByFlightNumber(1)).ReturnsAsync(new Flight());
            
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
        
        [TestMethod]
        public async Task CreateBooking_Failure_FlightNotInDatabase(){
            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlightByFlightNumber(1)).Throws(new FlightNotFoundException());
            (bool result, Exception exception) = await _bookService.CreateBooking("Rin", 1);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }

        //success because we created a customer for the case that they are not in the database.
        // the exception return type is CustomerNotFound b/c all calls to GetCustomerByName throws that exception.
        // in a real run, after the exception is caught, the customer will be added to db and the booking would be created.
        [TestMethod]
        public async Task CreateBooking_Success_CustomerNotInDatabase(){
            _mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(0,0)).Returns(Task.CompletedTask);
            _mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("Alice")).Throws(new CustomerNotFoundException());
            (bool result, Exception exception) = await _bookService.CreateBooking("Alice", 0);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CustomerNotFoundException));
        }

        //the case that bookingRepo throws an exception from bookingService's call.
        [TestMethod]
        public async Task CreateBooking_Failure_CustomerNotInDatabase_RepositoryFailure(){
            _mockBookRepo.Setup(bookRepo => bookRepo.CreateBooking(0,0)).Throws(new CouldNotAddBookingToDatabaseException());
            _mockCustRepo.Setup(custRepo => custRepo.GetCustomerByName("Alice")).ReturnsAsync(new Customer("Alice"){CustomerId = 0});
            (bool result, Exception exception) = await _bookService.CreateBooking("Alice", 0);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }
    }
}