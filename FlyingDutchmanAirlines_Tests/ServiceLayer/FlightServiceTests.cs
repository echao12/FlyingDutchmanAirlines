using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using System.Collections.Generic;
using Moq;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer{
    [TestClass]
    public class FlightServiceTest{
        Mock<FlightRepository> _mockFlightRepo;
        Mock<AirportRepository> _mockAirportRepo;

        [TestInitialize]
        public void TestInitialize(){
            _mockFlightRepo = new Mock<FlightRepository>();
            _mockAirportRepo = new Mock<AirportRepository>();
            //setup the return value for GetFlights()
            Flight flightInDb = new Flight {
                FlightNumber = 148,
                Origin = 31,
                Destination = 92
            };
            Queue<Flight> mockReturn = new Queue<Flight>(1);
            mockReturn.Enqueue(flightInDb);
            //setup the mock fns to throw an FlightNotFound from looking through airportIds
            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlights()).Returns(mockReturn);
            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlightByFlightNumber(148)).Returns(Task.FromResult(flightInDb));
            //setup airports to return
            _mockAirportRepo.Setup(airportRepo => airportRepo.GetAirportById(31))
                .ReturnsAsync(new Airport 
                    {
                        AirportId = 31,
                        City = "Mexico City",
                        Iata = "MEX"
                    });
            _mockAirportRepo.Setup(airportRepo => airportRepo.GetAirportById(92))
                .ReturnsAsync(new Airport
                    {
                        AirportId = 92,
                        City = "Ulaanbaatar",
                        Iata = "UBN"
                    });
        }

        [TestMethod]
        public async Task GetFlights_Success() {

            FlightService flightService = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);
            await foreach(FlightView flightView in flightService.GetFlights()){
                Assert.IsNotNull(flightView);
                Assert.AreEqual(flightView.FlightNumber, "148");
                Assert.AreEqual(flightView.Origin.City, "Mexico City");
                Assert.AreEqual(flightView.Origin.Code, "MEX");
                Assert.AreEqual(flightView.Destination.City, "Ulaanbaatar");
                Assert.AreEqual(flightView.Destination.Code, "UBN");
            };
        }
        
        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlights_Failure_RepositoryException(){
            _mockAirportRepo.Setup(airportRepo => airportRepo.GetAirportById(31)).ThrowsAsync(new FlightNotFoundException());

            FlightService flightService = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);
            
            //using discard(_) because we dont need the values returned.
            await foreach(FlightView _ in flightService.GetFlights()){
                ; // no logic. just testing the getFlights()
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFlights_Failure_RegularException() {
            FlightService flightService = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);
            _mockAirportRepo.Setup(airportRepo => airportRepo.GetAirportById(31)).ThrowsAsync(new ArgumentException());
            await foreach(FlightView _ in flightService.GetFlights()){
                ;
            };
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Success() {
            FlightService flightService = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);
            FlightView flightView = await flightService.GetFlightByFlightNumber(148);
            Assert.IsNotNull(flightView.FlightNumber, "148");
            Assert.AreEqual(flightView.Origin.City, "Mexico City");
            Assert.AreEqual(flightView.Origin.Code, "MEX");
            Assert.AreEqual(flightView.Destination.City, "Ulaanbaatar");
            Assert.AreEqual(flightView.Destination.Code, "UBN");
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByFlightNumber_Failure_RepositoryException_FlightNotFoundException(){
            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlightByFlightNumber(-1)).Throws(new FlightNotFoundException());
            FlightService service = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);
            await service.GetFlightByFlightNumber(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFlightByFlightNumber_Failure_RepositoryException_Exception() {
            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlightByFlightNumber(-1)).Throws(new OverflowException());
            FlightService service = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);

            await service.GetFlightByFlightNumber(-1);
        }
    }
}