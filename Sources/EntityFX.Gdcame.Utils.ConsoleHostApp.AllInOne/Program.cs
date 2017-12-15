using System.Configuration;
using Autofac;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Utils.Common;
using Topshelf;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServer
{
    internal class Program
    {
        private static void Main()
        {
            /*HostFactory.Run(configureCallback =>
            {*/

                var runtimeHelper = new RuntimeHelper();


            var appConfiguration = new AppConfiguration()
            {
                MongoConnectionString = ConfigurationManager.AppSettings["MongoConnectionString"],
                RepositoryProvider = ConfigurationManager.AppSettings["RepositoryProvider"] ?? "Mongo",
                WebApiPort = int.Parse(ConfigurationManager.AppSettings["WebApiPort"] ?? "80"),
                SignalRPort = int.Parse(ConfigurationManager.AppSettings["WebApiPort"] ?? "80"),
                WebServer = ConfigurationManager.AppSettings["WebServer"] ?? "Kestrel",
                KestrelThreads = int.Parse(ConfigurationManager.AppSettings["KestrelThreads"] ?? "32")
            };

            var host = new HostService(runtimeHelper, appConfiguration, new AutofacIocContainer(new ContainerBuilder()));
            host.Start();

            //configureCallback.RunAsNetworkService();
            //configureCallback.StartAutomatically();
            //configureCallback.UseNLog();
            //configureCallback.SetServiceName("GDCame");
            /*configureCallback.Service<HostService>(service =>
            {
                service.ConstructUsing(_ => new HostService(runtimeHelper, iocContainer, serviceProvider, appConfiguration));
                service.WhenStarted((hostService, control) => hostService.Start(control));
                service.WhenStopped((hostService, control) => hostService.Stop(control));
            });*/
            /*configureCallback.EnableServiceRecovery(configurator =>
            {
                configurator.RestartService(1);
                configurator.RestartComputer(10, "Holly shit: gdcame not respond too much - restart computer");
            });*/
            /*});*/

        }
    }
}