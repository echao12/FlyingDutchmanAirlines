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
        }

        //Config Services is called by host first before Configure().
        //add services to the application to use.
        public void ConfigureServices(IServiceCollection services){
            services.AddControllers();
        }
    }
}