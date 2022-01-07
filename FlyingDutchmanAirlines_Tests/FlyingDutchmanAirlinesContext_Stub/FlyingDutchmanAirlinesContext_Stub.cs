using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.Stubs {
    class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext {
        public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options)
                    : base(options) {
                base.Database.EnsureDeleted();//checks if in-memory database is deleted. if not, it will delete it.
            }
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            // check pending changes to db for changes to be added.
            IEnumerable<EntityEntry> pendingChanges = 
                ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            //fetch only bookings from pending changes
            IEnumerable<Booking> bookings = pendingChanges.Select(e => e.Entity).OfType<Booking>();
            if(bookings.Any(b => b.CustomerId != 1)){
                throw new Exception("Database Error for Bookings!");
            }

            IEnumerable<Airport> airports = pendingChanges.Select(e => e.Entity).OfType<Airport>();
            if(airports.Any(x => x.AirportId == 10)) {
                throw new Exception("Database Error for Airports!");
            }

            //note: cancellation token can cancel database queries by calling cancellationtoken.cancel()
            //if we dont pass in cancellationtoken, CLR will auto-assign a new instance of it.
            
            //invoking the base's saveChanges method now.
            await base.SaveChangesAsync(cancellationToken);
            
            return 0;

        }
    }
}