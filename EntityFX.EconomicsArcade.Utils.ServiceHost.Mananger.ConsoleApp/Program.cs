using EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager;
using System;
using System.Configuration;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.Mananger.ConsoleApp
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
            ss.StartService();
            Console.ReadKey();
        }
    }
}
