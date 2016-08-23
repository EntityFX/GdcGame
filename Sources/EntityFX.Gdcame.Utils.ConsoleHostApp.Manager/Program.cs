﻿using System;
using System.Configuration;
using EntityFX.Gdcame.Utils.ServiceStarter.Manager;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Manager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new ManagerStarter(
                containerBootstrapper
                , ConfigurationManager.AppSettings["ManagerEndpoint_BaseAddressServiceUrl"]
                );
            ss.StartServices();
            Console.ReadKey();
        }
    }
}