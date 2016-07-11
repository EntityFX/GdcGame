using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.ConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new NotifyConsumerStarter(
                containerBootstrapper, ConfigurationManager.AppSettings["NotifyConsumerEndpoint_BaseAddressServiceUrl"],
                 ConfigurationManager.AppSettings["NotifyConsumerSignalRHubEndpoint_AddressServiceUrl"]
                );
            ss.StartService();
            Console.ReadKey();
        }
    }

}