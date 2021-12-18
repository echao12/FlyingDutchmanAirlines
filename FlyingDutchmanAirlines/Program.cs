using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;//for IHost

namespace FlyingDutchmanAirlines
{
    class Program
    {
        static void Main(string[] args) // etry point to all C# programs
        {
            // note: Host lives inside common language runtime. Web service lives inside host.
            //create a host to run the web service.
            //host process is responsible for app startup & lifetime management.
            //minimum req: Host configures server & request processing pipelines
            IHost host = 
                Host.CreateDefaultBuilder() //returns a hostbulder with default settings
                        .ConfigureWebHostDefaults( builder =>
                            {
                                builder.UseUrls("http://0.0.0.0:8080"); //specify url&port we wanna use for this builder
                            }).Build(); //build & return resulting host.
                host.Run(); // start host
        }
    }
}
