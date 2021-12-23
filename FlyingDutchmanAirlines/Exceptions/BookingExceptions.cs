using System;
namespace FlyingDutchmanAirlines.Exceptions {
    public class CouldNotAddBookingToDatabaseException : CouldNotAddEntityToDatabaseException {}
    public class CouldNotAddEntityToDatabaseException : Exception {}
}