using System.Configuration;
using System.ServiceProcess;
using EntityFX.Gdcame.Utils.ServiceStarter.Manager;

namespace EntityFX.GdCame.Utils.WindowsHostSrv.Manager
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var dataAccessStarter = new ManagerStarter(
                containerBootstrapper
                , ConfigurationManager.AppSettings["ManagerEndpoint_BaseAddressServiceUrl"]
                //, ConfigurationManager.AppSettings["DataAccessHost_BaseStoreUri"]
                );
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WindowsServiceHostManager(dataAccessStarter)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}