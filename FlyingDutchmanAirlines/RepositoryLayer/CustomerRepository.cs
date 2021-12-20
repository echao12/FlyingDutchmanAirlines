using System.Linq;

namespace FlyingDutchmanAirlines.RepositoryLayer {
    public class CustomerRepository {
        public bool CreateCustomer(string name) {
            if(IsInvalidCustomerName(name)){
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