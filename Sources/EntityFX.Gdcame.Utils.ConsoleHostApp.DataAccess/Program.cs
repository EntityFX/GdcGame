using System;
using System.Configuration;
using EntityFX.Gdcame.Utils.ServiceStarter.DataAccess;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.DataAccess
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
            ss.StartServices();
            Console.ReadKey();
        }
    }
}
