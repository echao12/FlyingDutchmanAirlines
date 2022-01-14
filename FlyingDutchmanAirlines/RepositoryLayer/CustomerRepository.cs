using System.Collections.Generic;// for comparer class
using System.Diagnostics.CodeAnalysis;// for [DisallowNull] attribute
using System.Linq;
using System.Security.Cryptography; // for random number generator
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class CustomerRepository {
        private readonly FlyingDutchmanAirlinesContext _context;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public CustomerRepository(){
            if(Assembly.GetCallingAssembly().FullName == Assembly.GetExecutingAssembly().FullName){
                throw new System.Exception("This constructor should only be used for testing");
            }
        }
        public CustomerRepository(FlyingDutchmanAirlinesContext context) {
            this._context = context;
        }
        public async Task<bool> CreateCustomer(string name) {
            if(IsInvalidCustomerName(name)){
                return false;
            }
            try {
            Customer newCustomer = new Customer(name);
            //note: DbContext impolements IDisposable so we needa explicitly dispose of it or it will have a connection for infinite time
            // "using" statement is good for IDisposable objects. It defines a scope where the object will be disposed at the end of it.
            using (_context){ //creating var/scope for context
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();
            }//context is auto disposed here
            } catch {
                return false;
            }
            return true;
        }

        private bool IsInvalidCustomerName(string str){
            char[] forbiddenCharacters = {'!', '@', '#', '$', '%', '&', '*'};
            //check for null/empty, then use linq to check each char for an invalid character.
            return string.IsNullOrEmpty(str) || 
                str.Any(x => forbiddenCharacters.Contains(x));
        }

        public virtual async Task<Customer> GetCustomerByName(string name){
            if(IsInvalidCustomerName(name)){
                throw new CustomerNotFoundException();
            }
            return await _context.Customers.FirstOrDefaultAsync(c => c.Name == name)
                ?? throw new CustomerNotFoundException(); // recall '??' is null coalescing operator. returns the right value instead if result is null.
        }


    }
}