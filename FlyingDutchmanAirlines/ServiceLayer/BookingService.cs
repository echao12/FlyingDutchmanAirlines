using FlyingDutchmanAirlines.RepositoryLayer;
using System.Threading.Tasks;
using System;

namespace FlyingDutchmanAirlines.ServiceLayer{
    public class BookingService{
        private readonly BookingRepository _BookingRepo;

        public BookingService(BookingRepository repo){
            this._BookingRepo = repo;
        }
    }
}