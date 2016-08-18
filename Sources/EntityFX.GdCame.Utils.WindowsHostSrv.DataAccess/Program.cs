using EntityFX.Gdcame.Utils.ServiceStarter.DataAccess;
using System.Configuration;
using System.ServiceProcess;


namespace EntityFX.GdCame.Utils.WindowsHostSrv.DataAccess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
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
