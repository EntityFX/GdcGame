﻿using System;
using EntityFX.Gdcame.Utils.ServiceStarter.Collapsed;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Collapsed
{
    class Program
    {
        static void Main(string[] args)
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
