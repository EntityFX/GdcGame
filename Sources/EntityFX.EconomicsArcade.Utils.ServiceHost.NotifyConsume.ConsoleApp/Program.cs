using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.ConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new NotifyConsumerStarter(
                containerBootstrapper, ConfigurationManager.AppSettings["NotifyConsumerEndpoint_BaseAddressServiceUrl"],
                 ConfigurationManager.AppSettings["NotifyConsumerSignalRHubEndpoint_AddressServiceUrl"]
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }

}