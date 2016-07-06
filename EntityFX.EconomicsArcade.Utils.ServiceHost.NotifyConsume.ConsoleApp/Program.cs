using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.ConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new NotifyConsumerStarter(
                containerBootstrapper, ConfigurationManager.AppSettings["NotifyConsumerEndpoint_BaseAddressServiceUrl"]
                );
            ss.StartService();

            string url = ConfigurationManager.AppSettings["NotifyConsumerSignalRHubEndpoint_AddressServiceUrl"];
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}