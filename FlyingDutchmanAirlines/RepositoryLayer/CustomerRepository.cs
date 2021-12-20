using System.Linq;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class CustomerRepository {
        public async Task<bool> CreateCustomer(string name) {
            if(IsInvalidCustomerName(name)){
                return false;
            }
            Customer newCustomer = new Customer(name);
            //note: DbContext impolements IDisposable so we needa explicitly dispose of it or it will have a connection for infinite time
            // "using" statement is good for IDisposable objects. It defines a scope where the object will be disposed at the end of it.
            using (FlyingDutchmanAirlinesContext context = new FlyingDutchmanAirlinesContext()){ //creating var/scope for context
                context.Customers.Add(newCustomer);
                await context.SaveChangesAsync();
            }//context is auto disposed here
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