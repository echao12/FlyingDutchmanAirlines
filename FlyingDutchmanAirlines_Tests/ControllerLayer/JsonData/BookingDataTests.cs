using System;
using FlyingDutchmanAirlines.JsonData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.JsonData {
    [TestClass]
    public class BookingDataTests {
        [TestMethod]
        public void BookingData_ValidData(){
            BookingData bookingData = new BookingData{
                FirstName = "Marina",
                LastName = "Michaels"
            };

            Assert.IsNotNull(bookingData);
            Assert.AreEqual("Marina", bookingData.FirstName);
            Assert.AreEqual("Michaels", bookingData.LastName);
        }
        [TestMethod]
        [DataRow("mclovin", null)]
        [DataRow(null, "Mike")]
        [DataRow(null, "")]
        [DataRow(null, null)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BookingData_InvalidData_NullPointers(string firstName, string lastName){
            BookingData data = new BookingData {
                FirstName = firstName,
                LastName = lastName
            };
            Assert.AreEqual(firstName, data.FirstName);
            Assert.AreEqual(lastName, data.LastName);
        }

        [TestMethod]
        [DataRow("", "Long")]
        [DataRow("Jake", "")]
        [DataRow("", null)]
        [DataRow("", "")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BookingData_InvalidData_EmptyStrings(string firstName, string lastName){
            BookingData data = new BookingData {
                FirstName = firstName,
                LastName = lastName
            };
            Assert.AreEqual(firstName, data.FirstName);
            Assert.AreEqual(lastName, data.LastName);
        }
    }
}