using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Workermanager;
using EntityFX.Gdcame.Manager.Workers;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOneCore;
using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Topshelf;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    internal class HostService : ServiceControl
    {
        private readonly StartOptions _webApiStartOptions;
        private readonly ILogger _currentClassLogger;
        private IWebHost _webHost;
        private AppConfiguration AppConfiguration { get; }


        public HostService()
        {
            CoreStartup.AppConfiguration = new AppConfiguration()
            {
                MongoConnectionString = ConfigurationManager.AppSettings["MongoConnectionString"],
                RepositoryProvider = ConfigurationManager.AppSettings["RepositoryProvider"],
                WebApiPort = int.Parse(ConfigurationManager.AppSettings["WebApiPort"]),
                SignalRPort = int.Parse(ConfigurationManager.AppSettings["WebApiPort"]),
            };
            AppConfiguration = CoreStartup.AppConfiguration;

            CoreStartup.Container = (new ContainerBootstrapper(AppConfiguration)).Configure(new UnityContainer());
            ConfigureWorkers(CoreStartup.Container.Resolve<IWorkerManager>(),CoreStartup.Container);


            _currentClassLogger = CoreStartup.Container.Resolve<ILogger>();
            _webApiStartOptions = new StartOptions
            {
                Port = CoreStartup.AppConfiguration.WebApiPort,
            };

            if (RuntimeHelper.IsRunningOnMono())
            {
                //webApiStartOptions.ServerFactory = "Nowin";
                _webApiStartOptions.Urls.Add(string.Format("http://+:{0}", _webApiStartOptions.Port));
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    _webApiStartOptions.Urls.Add(string.Format("http://127.0.0.1:{0}", _webApiStartOptions.Port));
                }
            }
            else
            {
                _webApiStartOptions.Urls.Add(string.Format("http://+:{0}", _webApiStartOptions.Port));
            }

            _webHost = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.MaxRequestBufferSize = 4*1014*1024;
                    options.UseConnectionLogging();
                    options.ThreadCount = Environment.ProcessorCount;
                })
                .UseStartup<CoreStartup>()
                .UseUrls(_webApiStartOptions.Urls.ToArray())
                .Build();
        }

        public bool Start(HostControl hostControl)
        {
            _webHost.Start();

            StartWorkers(CoreStartup.Container.Resolve<IWorkerManager>());

            _currentClassLogger.Info(RuntimeHelper.GetRuntimeInfo());
            _currentClassLogger.Info("SignalR server running on {0}", CoreStartup.AppConfiguration.WebApiPort);
            _currentClassLogger.Info("Web server running on {0}", AppConfiguration.WebApiPort);
            _currentClassLogger.Info("Repository provider: {0}", CoreStartup.AppConfiguration.RepositoryProvider);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }

        public void ConfigureWorkers(IWorkerManager workerManager, IUnityContainer container)
        {
            workerManager.Add(container.Resolve<CalculationWorker>());
            workerManager.Add(container.Resolve<PersistenceWorker>());
            workerManager.Add(container.Resolve<SessionValidationWorker>());
        }

        public void StartWorkers(IWorkerManager workerManager)
        {
            workerManager.StartAll();
        }
    }
}