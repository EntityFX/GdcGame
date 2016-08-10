using System;
using System.Configuration;
using EntityFX.Gdcame.Utils.ServiceStarter.Collapsed;


namespace EntityFX.Gdcame.Utils.ConsoleHostApp.FullCollapsed
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new NoWcfContainerBootstrapper();
            var ss = new NoWcfServiceStarter(
                containerBootstrapper,
                 ConfigurationManager.AppSettings["NotifyConsumerSignalRHubEndpoint_AddressServiceUrl"]
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}
