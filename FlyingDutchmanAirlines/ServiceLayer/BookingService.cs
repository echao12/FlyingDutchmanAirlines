using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using System.Threading.Tasks;
using System;
using System.Runtime.ExceptionServices;

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
                return (false, new ArgumentException("Invalid Arguments!"));
            }
            //verify flightId & fetch customer
            try{
                Customer customer = await GetCustomerFromDatabase(customerName) 
                    ?? await AddCustomerToDatabase(customerName);
                if(await FlightExistsInDatabase(flightId) == false){
                    return (false, new CouldNotAddBookingToDatabaseException());
                }
                //create the booking
                try{
                    await _bookingRepo.CreateBooking(customer.CustomerId, flightId);
                    return (true, null);
                }catch(CouldNotAddBookingToDatabaseException exception){
                    return(false, exception);
                }
            }catch(Exception exception){
                //error creating the booking
                return (false, exception);
            }
        }

        private async Task<bool> FlightExistsInDatabase(int flightId){
            try{
                return (await _flightRepo.GetFlightByFlightNumber(flightId) != null);
            }catch(FlightNotFoundException){
                return false;
            }
        }

        private async Task<Customer> GetCustomerFromDatabase(string name){
            try{
                return await _customerRepo.GetCustomerByName(name);
            }catch(CustomerNotFoundException){
                return null;
            }catch(ArgumentException exception){
                //rethrow the exception with the stack trace data intact.
                ExceptionDispatchInfo.Capture(exception.InnerException ?? new Exception()).Throw();
                return null;
            }
        }

        private async Task<Customer> AddCustomerToDatabase(string name){
            await _customerRepo.CreateCustomer(name);
            return await _customerRepo.GetCustomerByName(name);
        }
    }
}