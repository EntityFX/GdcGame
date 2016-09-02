using System;
using System.Configuration;
using EntityFX.Gdcame.Utils.ServiceStarter.WcfDataAccess;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.WcfDataAccess
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new DataAccessStarter(
                containerBootstrapper
                , ConfigurationManager.AppSettings["DataAccessHost_BaseUri"]
                , ConfigurationManager.AppSettings["DataAccessHost_BaseStoreUri"]
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}