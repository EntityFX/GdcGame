using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;
using System.Configuration;
using Owin;
using Microsoft.Owin.Cors;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.Temp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var notifyConsumerStarter = new NotifyConsumerStarter(
                containerBootstrapper, ConfigurationManager.AppSettings["NotifyConsumerEndpoint_BaseAddressServiceUrl"]
                );

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new WindowsServiceHostNotifyConsumer(notifyConsumerStarter) 
            };
            ServiceBase.Run(ServicesToRun);
        }
        class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);
                app.MapSignalR();
            }
        }
    }
}
