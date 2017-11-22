using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin.Hosting;
using Topshelf;
using Unity;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.RatingServer
{
    class Program
    {
        static void Main(string[] args)
        {

            var runtimeHelper = new RuntimeHelper();

            var appConfiguration = new AppConfiguration();

            var host = new HostService(runtimeHelper, appConfiguration, new AutofacIocContainer(new ContainerBuilder()));
            host.Start();

            /*HostFactory.Run(configureCallback =>
            {
                configureCallback.RunAsNetworkService();
                configureCallback.StartAutomatically();
                configureCallback.UseNLog();
                configureCallback.SetServiceName("GDCameRatingServer");
                configureCallback.Service<HostService>(service =>
                {
                    service.ConstructUsing(_ => new HostService());
                    service.WhenStarted((hostService, control) => hostService.Start(control));
                    service.WhenStopped((hostService, control) => hostService.Stop(control));
                });
                configureCallback.EnableServiceRecovery(configurator =>
                {
                    configurator.RestartService(1);
                    configurator.RestartComputer(10, "Holly shit: gdcame not respond too much - restart computer");
                });
            });*/
        }
    }
}
