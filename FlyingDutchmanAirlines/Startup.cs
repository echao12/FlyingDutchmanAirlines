using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Builder; // for IApplicationBuilder class
using Microsoft.Extensions.DependencyInjection;//for IServicesCollection

namespace FlyingDutchmanAirlines {
    class Startup {
        //configure called by HostBuilder & contains configuration info about the app.
        public void Configure(IApplicationBuilder app) {
            //enable routing to endpoints
            app.UseRouting();
            //enables ability for us to use/specify endpoints
            //takes Action type as an argument. Action is a delegate.
                //NOTE: delegates is a way to reference methods. can only point to methods with a given signature.
                //delegates created in 3 ways: delegate keyword, anonymous method, lambda expression.
                //we are using a lambda expression for this Action delegate.

            //Map.Controllers scans codebase for any controllers & generates the appropriate routes to the endpoints in the controllers.
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Flying Dutchman Airlines")
            );
        }

        //Config Services is called by host first before Configure().
        //add services to the application to use.
        public void ConfigureServices(IServiceCollection services){
            services.AddControllers();
            // add dependencies by [dependencytype].(requested, injected) types.
            //inject dependencies for the flight controller
            services.AddTransient(typeof(FlightService), typeof(FlightService));
            
            //inject dependencies required for the FlightService
            services.AddTransient(typeof(FlightRepository), typeof(FlightRepository));
            services.AddTransient(typeof(AirportRepository), typeof(AirportRepository));
            services.AddTransient(typeof(FlightRepository), typeof(FlightRepository));
            services.AddTransient(typeof(CustomerRepository), typeof(CustomerRepository));
            services.AddTransient(typeof(BookingRepository), typeof(BookingRepository));
            services.AddDbContext<FlyingDutchmanAirlinesContext>(ServiceLifetime.Transient);
            //inject dependencies required for the above repositories
            services.AddTransient(typeof(FlyingDutchmanAirlinesContext), typeof(FlyingDutchmanAirlinesContext));

            services.AddSwaggerGen();
        }
    }
}