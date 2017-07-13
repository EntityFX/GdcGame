using Topshelf;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServer
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(configureCallback =>
            {
                configureCallback.RunAsNetworkService();
                configureCallback.StartAutomatically();
                configureCallback.UseNLog();
                configureCallback.SetServiceName("GDCame");
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
            });

        }
    }
}