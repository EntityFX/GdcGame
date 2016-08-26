using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    internal class Program
    {
        private static void Main()
        {
            var baseHost = "+";

            var webApiPort = "9001";

            var signalRPort = "9091";


            // Start OWIN host 
            var httpWebApi = WebApp.Start<Startup>(string.Format("http://{0}:{1}", baseHost, webApiPort));

            var signalR = WebApp.Start(string.Format("http://{0}:{1}", baseHost, signalRPort), builder =>
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
            Console.WriteLine("SignalR server running on {0}", signalRPort);
            Console.WriteLine("Web server running on {0}", webApiPort);


            Console.ReadLine();
        }
    }
}