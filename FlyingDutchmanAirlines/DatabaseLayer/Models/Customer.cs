using System;
using System.Collections.Generic;

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


    }
}
