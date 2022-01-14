using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using System.Threading.Tasks;
using System;

namespace FlyingDutchmanAirlines.ServiceLayer{
    public class BookingService{
        private readonly BookingRepository _bookingRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly FlightRepository _flightRepo;

        public BookingService(BookingRepository bookRepo, CustomerRepository custRepo, FlightRepository flightRepo){
            this._bookingRepo = bookRepo;
            this._customerRepo = custRepo;
            this._flightRepo = flightRepo;
        }

        public async Task<(bool, Exception)> CreateBooking(string customerName, int flightId){
            if(String.IsNullOrEmpty(customerName) || !flightId.IsPositive()){
                throw new ArgumentException();
            }
            //fetch customer
            try{
                Customer customer;
                try{
                    customer = await _customerRepo.GetCustomerByName(customerName);
                }catch(CustomerNotFoundException){
                    //customer not found, create customer and try again.
                    await _customerRepo.CreateCustomer(customerName);
                    return await CreateBooking(customerName, flightId);
                }
                //create the booking
                await _bookingRepo.CreateBooking(customer.CustomerId, flightId);
                return (true, null);
            }catch(Exception exception){
                //error creating the booking
                return (false, exception);
            }
        }
    }
}