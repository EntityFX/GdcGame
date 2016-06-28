using EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess;
using System;
using System.Configuration;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new DataAccessStarter(
                containerBootstrapper
                , ConfigurationManager.AppSettings["DataAccessHost_BaseUri"]
                , ConfigurationManager.AppSettings["DataAccessHost_BaseStoreUri"]
                );
            ss.StartService();
            Console.ReadKey();
        }
    }
}
