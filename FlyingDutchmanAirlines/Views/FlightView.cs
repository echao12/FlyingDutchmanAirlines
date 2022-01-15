namespace FlyingDutchmanAirlines.Views{

    public class FlightView {
        public string FlightNumber { get; private set; }
        public AirportInfo Origin { get; private set; }
        public AirportInfo Destination { get; private set; }

        public FlightView(string flightNumber, (string city, string code) origin, (string city, string code) destination) {
            FlightNumber = string.IsNullOrWhiteSpace(flightNumber) ? "Flight Number Not Found" : flightNumber;
            Origin = new AirportInfo(origin);
            Destination = new AirportInfo(destination);
        }
    }
    public struct AirportInfo {
        public string City {get; set;}
        public string Code {get; set;}
        public AirportInfo((string city, string code) airport) {
            City = string.IsNullOrWhiteSpace(airport.city) ? "City Not Found" : airport.city;
            Code = string.IsNullOrWhiteSpace(airport.code) ? "City Code Not Found" : airport.code;
        }
    }
}