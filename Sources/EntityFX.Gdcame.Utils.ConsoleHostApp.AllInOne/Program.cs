using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Utils.Common;
using Topshelf;
using Unity;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServer
{
    internal class Program
    {
        private static void Main()
        {
            /*HostFactory.Run(configureCallback =>
            {*/
                var iocContainer = new UnityIocContainer(new UnityContainer());

                var runtimeHelper = new RuntimeHelper();

                var serviceProvider = new UnityDependencyProvider(iocContainer.Container);

                var appConfiguration = new AppConfiguration();

            var host = new HostService(runtimeHelper, iocContainer, serviceProvider, appConfiguration);
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