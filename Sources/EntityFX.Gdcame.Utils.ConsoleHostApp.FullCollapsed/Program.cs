using System;
using System.Configuration;
using EntityFX.Gdcame.Utils.ServiceStarter.Collapsed;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.FullCollapsed
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var containerBootstrapper = new FullCollapsedContainerBootstrapper();
            var ss = new FullCollapsedServiceStarter(
                containerBootstrapper,
                ConfigurationManager.AppSettings["NotifyConsumerSignalRHubEndpoint_AddressServiceUrl"]
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}