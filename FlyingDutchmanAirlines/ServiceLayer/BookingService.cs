using FlyingDutchmanAirlines.RepositoryLayer;
using System.Threading.Tasks;
using System;

namespace FlyingDutchmanAirlines.ServiceLayer{
    public class BookingService{
        private readonly BookingRepository _BookingRepo;
        private readonly CustomerRepository _CustomerRepo;

        public BookingService(BookingRepository bookRepo){
            this._BookingRepo = bookRepo;
        }

        public async Task<(bool, Exception)> CreateBooking(string customerName, int flightId){
            return (true, null);
        }
    }
}