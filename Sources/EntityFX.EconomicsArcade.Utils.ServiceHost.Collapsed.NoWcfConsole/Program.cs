using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.Collapsed;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.Collapsed.NoWcfConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new NoWcfContainerBootstrapper();
            var ss = new NoWcfServiceStarter(
                containerBootstrapper
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}
