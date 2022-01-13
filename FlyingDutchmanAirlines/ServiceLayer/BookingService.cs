using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
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
            if(String.IsNullOrEmpty(customerName) || !flightId.IsPositive()){
                throw new ArgumentException();
            }
            //fetch customer
            try{
                Customer customer;
                try{
                    customer = await _CustomerRepo.GetCustomerByName(customerName);
                }catch(CustomerNotFoundException){
                    //customer not found, create customer and try again.
                    await _CustomerRepo.CreateCustomer(customerName);
                    return await CreateBooking(customerName, flightId);
                }
                //create the booking
                await _BookingRepo.CreateBooking(customer.CustomerId, flightId);
                return (true, null);
            }catch(Exception exception){
                //error creating the booking
                return (false, exception);
            }
        }
    }
}