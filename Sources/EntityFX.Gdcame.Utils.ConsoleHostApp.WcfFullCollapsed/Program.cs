using System;
using System.Configuration;
using EntityFX.Gdcame.Utils.ServiceStarter.WcfCollapsed;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.WcfFullCollapsed
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