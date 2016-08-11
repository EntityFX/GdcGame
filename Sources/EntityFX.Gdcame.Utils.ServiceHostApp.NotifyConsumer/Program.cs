using System.Configuration;
using System.ServiceProcess;
using EntityFX.Gdcame.Utils.ServiceStarter.NotifyConsumer;

namespace EntityFX.Gdcame.Utils.ServiceHostApp.NotifyConsumer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var notifyConsumerStarter = new NotifyConsumerStarter(
                containerBootstrapper, ConfigurationManager.AppSettings["NotifyConsumerEndpoint_BaseAddressServiceUrl"],
                 ConfigurationManager.AppSettings["NotifyConsumerSignalRHubEndpoint_AddressServiceUrl"]
                );

            var servicesToRun = new ServiceBase[] 
            { 
                new WindowsServiceHostNotifyConsumer(notifyConsumerStarter) 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
