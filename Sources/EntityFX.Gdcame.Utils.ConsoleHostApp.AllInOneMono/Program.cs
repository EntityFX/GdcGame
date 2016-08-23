using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOneMono
{
    internal class Program
    {
        private static void Main()
        {
            var baseAddress = "http://localhost:9001/";

            var _signalRHost = "http://localhost:9091/";

            // Start OWIN host 
            var httpWebApi = WebApp.Start<Startup>(baseAddress);

            var signalR = WebApp.Start(_signalRHost, builder =>
            {
                var listener = (HttpListener)builder.Properties[typeof(HttpListener).FullName];
                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                builder.UseCors(CorsOptions.AllowAll);
                builder.MapSignalR();
                builder.RunSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJSONP = true
                });
            });
            Console.WriteLine("SignalR server running on {0}", _signalRHost);
            Console.WriteLine("Web server running on {0}", baseAddress);

            var client = new HttpClient();

            var response = client.GetAsync(baseAddress + "api/heartbeat").Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);


            Console.ReadLine();
        }
    }
}