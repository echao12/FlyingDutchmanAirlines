using System.Linq;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class CustomerRepository {
        private readonly FlyingDutchmanAirlinesContext _context;

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
    }
}