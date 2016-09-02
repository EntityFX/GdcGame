using System.Configuration;
using System.ServiceProcess;
using EntityFX.Gdcame.Utils.ServiceStarter.WcfDataAccess;

namespace EntityFX.Gdcame.Utils.WindowsHostSrv.WcfDataAccess
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var dataAccessStarter = new DataAccessStarter(
                containerBootstrapper
                , ConfigurationManager.AppSettings["DataAccessHost_BaseUri"]
                , ConfigurationManager.AppSettings["DataAccessHost_BaseStoreUri"]
                );

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WindowsServiceHostDataAccess(dataAccessStarter)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}