using FlyingDutchmanAirlines.RepositoryLayer; //to access methods for testing
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        [TestMethod]
        public void CreateCustomer_Success() {
            CustomerRepository repository = new CustomerRepository();
            Assert.IsNotNull(repository);
        }
    }
}
