using FlyingDutchmanAirlines.DatabaseLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.Stubs {
    class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext {
        public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options)
                    : base(options) { }
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            //note: cancellation token can cancel database queries by calling cancellationtoken.cancel()
            //if we dont pass in cancellationtoken, CLR will auto-assign a new instance of it.
            
            //check to make sure first booking has customerId = 1. note: c# collections are zero-based. 
            if( (await base.Bookings.FirstAsync()).CustomerId != 1 ){
                throw new Exception("Database Error!");
            }
            //invoking the base's saveChanges method now.
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}