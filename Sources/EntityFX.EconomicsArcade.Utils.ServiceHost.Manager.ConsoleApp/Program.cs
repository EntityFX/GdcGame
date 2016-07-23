using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.Manager.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new ManagerStarter(
                containerBootstrapper
                , ConfigurationManager.AppSettings["ManagerEndpoint_BaseAddressServiceUrl"]
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}
