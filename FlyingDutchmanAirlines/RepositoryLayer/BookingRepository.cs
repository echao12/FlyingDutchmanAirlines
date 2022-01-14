using System.Threading.Tasks;
using System;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class BookingRepository {
        private readonly FlyingDutchmanAirlinesContext _context;

        //do not let compiler inline so it can fetch the calling assembly name from the prev. stack frame.
        //  if compiler inlines, there is a chance that the previous stack frame does not have the calling assembly.
        [MethodImpl(MethodImplOptions.NoInlining)]
        public BookingRepository() {
            //uses reflection to check if the calling assembly (which should be the testing env b/c it calls this fn) 
            //  and the executing assembly (which should be this assembly) b/c it is executing this line here.
            if(Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName){
                throw new Exception("This constructor should only be used for testing");
            }
        }
        public BookingRepository(FlyingDutchmanAirlinesContext context){
            this._context = context;
        }

        //note: to mock method calls, we need to make the original methods virtual so Mock can override. 
        public virtual async Task CreateBooking(int customerId, int flightNumber) {
            if(!customerId.IsPositive() || !flightNumber.IsPositive()) {
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