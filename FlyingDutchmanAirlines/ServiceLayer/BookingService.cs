using FlyingDutchmanAirlines.RepositoryLayer;
using System.Threading.Tasks;
using System;

namespace FlyingDutchmanAirlines.ServiceLayer{
    public class BookingService{
        private readonly BookingRepository _BookingRepo;
        private readonly CustomerRepository _CustomerRepo;

        public BookingService(BookingRepository bookRepo, CustomerRepository custRepo){
            this._BookingRepo = bookRepo;
            this._CustomerRepo = custRepo;
        }

        public async Task<(bool, Exception)> CreateBooking(string customerName, int flightId){
            return (true, null);
        }
    }
}