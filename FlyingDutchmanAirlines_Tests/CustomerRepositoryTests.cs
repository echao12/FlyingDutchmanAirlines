using System.Threading.Tasks;
using FlyingDutchmanAirlines.RepositoryLayer; //to access methods for testing
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines.DatabaseLayer;//for db context
using Microsoft.EntityFrameworkCore.InMemory;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        private FlyingDutchmanAirlinesContext _context;
        private CustomerRepository _repository;

        [TestInitialize]
        public async Task TestInitialize() {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = 
            // create new dbContextOptions using the builder pattern.
               new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                // specify builder to use in-memory db called "FlyingDutchman"
               .UseInMemoryDatabase("FlyingDutchman")
               //fetch all of the final configured options
               .Options;
            //use dbContextOptions to build & link a new dbContext to _context
            _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
            
            //add a customer in the database for unit tests to use.
            Customer testCustomer = new Customer("Rin");
            _context.Customers.Add(testCustomer);
            await _context.SaveChangesAsync();

            //initialize repository
            _repository = new CustomerRepository(_context);
            Assert.IsNotNull(_repository);
        }

        [TestMethod]
        public async Task CreateCustomer_Success() {
            //check for valid default object creation
            bool result = await _repository.CreateCustomer("Rin");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_DatabaseAccessError() {
            _repository = new CustomerRepository(null);
            Assert.IsNotNull(_repository);
            
            bool result = await _repository.CreateCustomer("Rin");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_NameIsNull() {
            bool result = await _repository.CreateCustomer(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_NameIsEmpty() {
            bool result = await _repository.CreateCustomer("");
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
            bool result = await _repository.CreateCustomer("Rin" + invalidCharacter);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetCustomerByName_Success() {
            Customer customer = await _repository.GetCustomerByName("Rin");
            Assert.IsNotNull(customer); //check for null
            
        }
        
        //force exception test. checking if the correct exception is thrown with invalid inputs.
        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("#")]
        [DataRow("$")]
        [DataRow("%")]
        [DataRow("&")]
        [DataRow("*")]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public async Task GetCustomerName_Failure_InvalidName(string name){
            await _repository.GetCustomerByName(name);
        }
    }
}
