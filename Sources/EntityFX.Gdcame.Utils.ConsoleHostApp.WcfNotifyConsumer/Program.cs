using System;
using System.Configuration;
using EntityFX.Gdcame.Utils.ServiceStarter.WcfNotifyConsumer;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.WcfNotifyConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
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