using System;
using EntityFX.Gdcame.Utils.ServiceStarter.Collapsed;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.Collapsed.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new WcfContainerBootstrapper();
            var ss = new CollapsedServiceStarter(
                containerBootstrapper
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}
