using System;
using EntityFX.Gdcame.Utils.ServiceStarter.Collapsed;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.FullCollapsed
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
