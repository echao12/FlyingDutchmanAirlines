using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingDutchmanAirlines.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlyingDutchmanAirlines_Tests.Controllers{
        [TestClass]
        public class FlightControllerTests{
            FlightController _flightController;
            FlightService _flightService;

            [TestInitialize]
            public Task TestInitialize(){
                _flightService = new FlightService();
            }
            [TestMethod]
            public Task GetFlights_Success(){
                _flightController = new FlightController();
                //this mimics a HTTP GET call to /Flight
                ObjectResult response = _flightController.GetFlights() as ObjectResult;

                Assert.IsNotNull(response);
                Assert.AreEqual((int) HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual("Hello, World!", response.Value);
            }
    }
}