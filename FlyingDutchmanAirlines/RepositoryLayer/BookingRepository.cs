using System.Threading.Tasks;
using System;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class BookingRepository {
        private readonly FlyingDutchmanAirlinesContext _context;

        public BookingRepository(FlyingDutchmanAirlinesContext context){
            this._context = context;
        }
        public async Task CreateBooking(int customerId, int flightNumber) {
            if(customerId < 0 || flightNumber < 0) {
                Console.WriteLine(
                    $"Argument Exception from CreateBooking!\nCustomer={customerId}, FlightNumber={flightNumber}"
                );
                throw new ArgumentException("Invalid arguments provided");
            }
            Booking newBooking = new Booking {
                CustomerId = customerId,
                FlightNumber = flightNumber
            };
            //try to add the booking
            try {
                _context.Bookings.Add(newBooking);
                await _context.SaveChangesAsync();
            } catch (Exception exception){
                Console.WriteLine($"Exception during database query: {exception.Message}");
                throw new CouldNotAddBookingToDatabaseException();
            }
        }
    }
}