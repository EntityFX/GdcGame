using EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
