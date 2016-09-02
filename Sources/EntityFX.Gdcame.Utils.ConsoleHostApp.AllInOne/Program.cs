using System;
using System.Configuration;
using System.Net;
using EntityFX.Gdcame.Utils.Common;
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


            var signalRPort = int.Parse(ConfigurationManager.AppSettings["SignalRPort"]);

            var webApiStartOptions = new StartOptions
            {
                Port = int.Parse(ConfigurationManager.AppSettings["WebApiPort"])
            };

            if (RuntimeHelper.IsRunningOnMono())
            {
                webApiStartOptions.ServerFactory = "Nowin";
            }

            // Start OWIN host 
            var httpWebApi = WebApp.Start<WebApiStartup>(webApiStartOptions);

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
            Console.WriteLine(RuntimeHelper.GetRuntimeInfo());
            Console.WriteLine("SignalR server running on {0}", signalRPort);
            Console.WriteLine("Web server running on {0}", webApiStartOptions.Port);


            Console.ReadLine();
        }
    }
}