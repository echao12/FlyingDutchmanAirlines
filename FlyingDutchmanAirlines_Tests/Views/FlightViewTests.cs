using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines_Tests.Views{
    [TestClass]
    public class FlightViewTests {
        [TestMethod]
        public void Constructor_FlightView_Success(){
            string flightNumber = "0";
            string originCity = "Amsterdam";
            string originCityCode = "AMS";
            string destinationCity = "Moscow";
            string destinationCityCode = "SVO";

            FlightView view = new FlightView(flightNumber, (originCity, originCityCode), (destinationCity, destinationCityCode));
            Assert.IsNotNull(view);
            Assert.AreEqual(view.FlightNumber, flightNumber);
            Assert.AreEqual(view.Origin, (originCity, originCityCode));
            Assert.AreEqual(view.Destination.City, destinationCity);
            Assert.AreEqual(view.Destination.Code, destinationCityCode);
        }

        [TestMethod]
        public void Constructor_FlightView_Success_FlightNumber_Null(){
            string originCity = "Athens";
            string originCityCode = "ATH";
            string destinationCity = "Dubai";
            string destinationCityCode = "DXB";
            FlightView flightView = new FlightView(null, (originCity, originCityCode), (destinationCity, destinationCityCode));
            Assert.IsNotNull(flightView);
            Assert.AreEqual(flightView.FlightNumber, "Flight Number Not Found");
            Assert.AreEqual(flightView.Origin.City, originCity);
            Assert.AreEqual(flightView.Origin.Code, originCityCode);
            Assert.AreEqual(flightView.Destination.City, destinationCity);
            Assert.AreEqual(flightView.Destination.Code, destinationCityCode);
        }

        [TestMethod]
        public void Constructor_AirportInfo_Success_City_EmptyString() {
            string destinationCity = string.Empty;
            string destinationCityCode = "SYD";

            AirportInfo airportInfo = new AirportInfo((destinationCity, destinationCityCode));
            Assert.IsNotNull(airportInfo);
            Assert.AreEqual(airportInfo.City, "City Not Found");
            Assert.AreEqual(airportInfo.Code, destinationCityCode);
        }

        [TestMethod]
        public void Constructor_AirportInfo_Success_Code_EmptyString(){
            string city = "Sacramento";
            string code = "";
            AirportInfo airportInfo = new AirportInfo((city, code));
            Assert.IsNotNull(airportInfo);
            Assert.AreEqual(airportInfo.City, city);
            Assert.AreEqual(airportInfo.Code, "City Code Not Found");
        }
    }
}