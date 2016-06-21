using EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess;
using System;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new DataAccessStarter(containerBootstrapper);
            ss.StartService();
            Console.ReadKey();
        }
    }
}
