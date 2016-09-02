using System;
using EntityFX.Gdcame.Utils.ServiceStarter.WcfCollapsed;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.WcfCollapsed
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var containerBootstrapper = new CollapsedContainerBootstrapper();
            var ss = new CollapsedServiceStarter(
                containerBootstrapper
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}