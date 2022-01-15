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
        
        FlightService _flightService;
        Mock<FlightRepository> _mockFlightRepo;
        Mock<AirportRepository> _mockAirportRepo;

        [TestInitialize]
        public void TestInitialize(){
            _mockFlightRepo = new Mock<FlightRepository>();
            _mockAirportRepo = new Mock<AirportRepository>();
        }

        [TestMethod]
        public async Task GetFlights_Success() {
            Flight flightInDb = new Flight {
                FlightNumber = 148,
                Origin = 31,
                Destination = 92
            };
            Queue<Flight> mockReturn = new Queue<Flight>(1);
            mockReturn.Enqueue(flightInDb);

            _mockFlightRepo.Setup(flightRepo => flightRepo.GetFlights()).Returns(mockReturn);
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
                        City = "Ulaanbaataar",
                        Iata = "UBN"
                    });
            FlightService flightService = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);
            await foreach(FlightView flightView in flightService.getFlights()){
                Assert.IsNotNull(flightView);
                Assert.AreEqual(flightView.FlightNumber, "148");
                Assert.AreEqual(flightView.Origin.City, "Mexico City");
                Assert.AreEqual(flightView.Origin.Code, "MEX");
                Assert.AreEqual(flightView.Destination.City, "Ulaanbaataar");
                Assert.AreEqual(flightView.Destination.Code, "UBN");
            }
        }
    }
}