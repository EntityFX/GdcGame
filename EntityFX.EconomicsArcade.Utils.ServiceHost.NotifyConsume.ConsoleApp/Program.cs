using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsume.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new NotifyConsumerStarter(
                containerBootstrapper, ConfigurationManager.AppSettings["NotifyConsumerEndpoint_BaseAddressServiceUrl"]
                );
            ss.StartService();
            Console.ReadKey();
        }
    }
}
