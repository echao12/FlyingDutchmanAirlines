using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer{
    [TestClass]
    public class FlightServiceTest{
        
        FlightService _flightService;
        FlightRepository _flightRepo;

        [TestInitialize]
        public void TestInitialize(){
        }
    }
}