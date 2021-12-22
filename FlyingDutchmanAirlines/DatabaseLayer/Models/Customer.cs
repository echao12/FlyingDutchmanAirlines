using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

#nullable disable

namespace FlyingDutchmanAirlines.DatabaseLayer.Models
{
    
    public sealed class Customer //sealed b/c we dont want any inheriting and modifying db access in a diff class
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        public ICollection<Booking> Bookings { get; set; }    
        public Customer(string name)
        {
            Bookings = new HashSet<Booking>();
            Name = name;
        }
        
        // magically calls comparer stuff for <Customer> types with these methods.
        internal class CustomerEqualityComparer : EqualityComparer<Customer>
        {
            public override bool Equals(Customer x, Customer y)
            {
                return (x.CustomerId == y.CustomerId) && (x.Name == y.Name);
            }

            public override int GetHashCode([DisallowNull] Customer obj)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue/2);
                return (obj.CustomerId + obj.Name.Length + randomNumber).GetHashCode();
            }
        }
    }
}
