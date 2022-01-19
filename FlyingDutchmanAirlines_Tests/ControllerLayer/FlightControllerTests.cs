using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingDutchmanAirlines.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Moq;
using System.Collections.Generic;
using FlyingDutchmanAirlines.Views;
using System.Linq;
using FlyingDutchmanAirlines.Exceptions;
using System;

namespace FlyingDutchmanAirlines_Tests.Controllers{
    [TestClass]
    public class FlightControllerTests{
        FlightController _flightController;
        Mock<FlightService> _flightService;

        [TestInitialize]
        public void TestInitialize(){
            //setup service mock
            _flightService = new Mock<FlightService>(); 
        }
        [TestMethod]
        public async Task GetFlights_Success(){
            //establish the views the mock will return
            List<FlightView> flightList = new List<FlightView>(2) {
                new FlightView("1932", ("Groningen", "GRQ"), ("Phoenix", "PHX")),
                new FlightView("841", ("New York City", "JFK"), ("London", "LHR"))
            };
            //setup the fn calls/returns
            _flightService.Setup(fService => fService.GetFlights()).Returns(FlightViewAsyncGenerator(flightList));
            //create flight controller using the service
            _flightController = new FlightController(_flightService.Object);
            //this mimics a HTTP GET call to /Flight and casts the result 
            //downcast GetFlights to compare status codes
            ObjectResult responseResult = await _flightController.GetFlights() as ObjectResult;
            Assert.IsNotNull(responseResult);
            Assert.AreEqual((int)HttpStatusCode.OK, responseResult.StatusCode);
            //convert data in ObjectResult to queue for comparison.
            Queue<FlightView> content = responseResult.Value as Queue<FlightView>;
            Assert.IsNotNull(content);
            Assert.IsTrue(flightList.All(flight => content.Contains(flight)));
        }

        [TestMethod]
        public async Task GetFlights_Failure_FlightNotFoundException_404(){
            _flightService.Setup(fService => fService.GetFlights()).Throws(new FlightNotFoundException());
            _flightController = new FlightController(_flightService.Object);
            ObjectResult result = await _flightController.GetFlights() as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("No flights were found in the database", result.Value);
        }

        [TestMethod]
        public async Task GetFlights_Failure_Exception_500(){
            _flightService.Setup(fService => fService.GetFlights()).Throws(new Exception());
            _flightController = new FlightController(_flightService.Object);
            ObjectResult result = await _flightController.GetFlights() as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("An error occurred", result.Value);
        }

        //converts an IEnumerable to IAsyncEnumerable by utilizing yield return
        private async IAsyncEnumerable<FlightView>FlightViewAsyncGenerator(IEnumerable<FlightView> flightList){
            foreach(FlightView flight in flightList){
                yield return flight;
            }
        }
        
        [TestMethod]
        public async Task GetFlightByFlightNumber_Success() {
            FlightView returnedView = new FlightView("0", ("Lagos", "LOS"), ("Marrakesh", "RAK"));
            _flightService.Setup(fService => fService.GetFlightByFlightNumber(0)).Returns(Task.FromResult(returnedView));

            FlightController controller = new FlightController(_flightService.Object);
            ObjectResult result = await controller.GetFlightByFlightNumber(0) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            
            FlightView content = result.Value as FlightView;
            Assert.IsNotNull(content);
            Assert.AreEqual(returnedView, content);
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Failure_FlightNotFoundException_404(){
            _flightService.Setup(fService => fService.GetFlightByFlightNumber(0)).Throws(new FlightNotFoundException());
            
            _flightController = new FlightController(_flightService.Object);

            ObjectResult response = await _flightController.GetFlightByFlightNumber(0) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("The flight was not found in the database", response.Value);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(1)]
        public async Task GetFlightByFlightNumber_Failure_ArgumentException_400(int flightnumber){
            _flightService.Setup(fService => fService.GetFlightByFlightNumber(1)).Throws(new ArgumentException());

            FlightController fController = new FlightController(_flightService.Object);
            ObjectResult response = await fController.GetFlightByFlightNumber(flightnumber) as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Bad request", response.Value);
        }
    }
}