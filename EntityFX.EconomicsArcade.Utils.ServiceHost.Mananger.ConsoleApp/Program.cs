using EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager;
using System;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.Mananger.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new ManagerStarter(containerBootstrapper);
            ss.StartService();
            Console.ReadKey();
        }
    }
}
