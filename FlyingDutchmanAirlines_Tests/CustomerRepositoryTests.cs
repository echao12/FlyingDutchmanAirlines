using System.Threading.Tasks;
using FlyingDutchmanAirlines.RepositoryLayer; //to access methods for testing
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines.DatabaseLayer;//for db context
using Microsoft.EntityFrameworkCore.InMemory;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        private FlyingDutchmanAirlinesContext _context;

        [TestInitialize]
        public void TestInitialize() {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = 
            // create new dbContextOptions using the builder pattern.
               new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                // specify builder to use in-memory db called "FlyingDutchman"
               .UseInMemoryDatabase("FlyingDutchman")
               //fetch all of the final configured options
               .Options;
            _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
        }

        [TestMethod]
        public async Task CreateCustomer_Success() {
            //check for valid default object creation
            CustomerRepository repository = new CustomerRepository(_context);
            Assert.IsNotNull(repository);

            bool result = await repository.CreateCustomer("Rin");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_DatabaseAccessError() {
            CustomerRepository repo = new CustomerRepository(null);
            Assert.IsNotNull(repo);

            bool result = await repo.CreateCustomer("Rin");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_NameIsNull() {
            CustomerRepository repo = new CustomerRepository(_context);
            Assert.IsNotNull(repo);

            bool result = await repo.CreateCustomer(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_NameIsEmpty() {
            CustomerRepository repo = new CustomerRepository(_context);
            Assert.IsNotNull(repo);

            bool result = await repo.CreateCustomer("");
            Assert.IsFalse(result);
        }

        //[DataRow] used to specify input parameter for test method.
        [TestMethod]
        [DataRow('#')]
        [DataRow('$')]
        [DataRow('%')]
        [DataRow('&')]
        [DataRow('*')]
        public async Task CreateCustomer_Failure_NameContainsInvalidCharacters(char invalidCharacter){
            CustomerRepository repo = new CustomerRepository(_context);
            Assert.IsNotNull(repo);

            bool result = await repo.CreateCustomer("Rin" + invalidCharacter);
            Assert.IsFalse(result);
        }
    }
}
