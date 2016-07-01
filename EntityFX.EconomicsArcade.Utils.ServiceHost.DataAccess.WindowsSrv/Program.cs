using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess;
using System.Configuration;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv
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
                new Service_ServiceHost_DataAccess(dataAccessStarter) 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
