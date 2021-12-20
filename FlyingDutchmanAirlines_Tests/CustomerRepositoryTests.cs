using FlyingDutchmanAirlines.RepositoryLayer; //to access methods for testing
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        [TestMethod]
        public void CreateCustomer_Success() {
            //check for valid default object creation
            CustomerRepository repository = new CustomerRepository();
            Assert.IsNotNull(repository);

            bool result = repository.CreateCustomer("Rin");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateCustomer_Failure_NameIsNull() {
            CustomerRepository repo = new CustomerRepository();
            Assert.IsNotNull(repo);

            bool result = repo.CreateCustomer(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateCustomer_Failure_NameIsEmpty() {
            CustomerRepository repo = new CustomerRepository();
            Assert.IsNotNull(repo);

            bool result = repo.CreateCustomer("");
            Assert.IsFalse(result);
        }

        //[DataRow] used to specify input parameter for test method.
        [TestMethod]
        [DataRow('#')]
        [DataRow('$')]
        [DataRow('%')]
        [DataRow('&')]
        [DataRow('*')]
        public void CreateCustomer_Failure_NameContainsInvalidCharacters(char invalidCharacter){
            CustomerRepository repo = new CustomerRepository();
            Assert.IsNotNull(repo);

            bool result = repo.CreateCustomer("Rin" + invalidCharacter);
            Assert.IsFalse(result);
        }
    }
}
