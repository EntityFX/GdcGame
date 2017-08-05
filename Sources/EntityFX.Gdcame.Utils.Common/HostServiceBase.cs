namespace EntityFX.Gdcame.Utils.Common
{
    using System;
    using System.Linq;
    using System.Threading;

    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Owin.Hosting;
    using Microsoft.Practices.Unity;

    using Topshelf;

    public abstract class HostServiceBase<TCoreStartup> : ServiceControl
        where TCoreStartup : CoreStartupBase
    {
        protected AppConfiguration AppConfiguration { get; private set; }

        private readonly StartOptions _webApiStartOptions;

        protected readonly ILogger Logger;

        protected IWebHost WebHost;

        private IUnityContainer _container;

        public HostServiceBase()
        {
            this.AppConfiguration = new AppConfiguration();
            CoreStartupBase.AppConfiguration = this.AppConfiguration;
            this._container = (this.GetContainerBootstrapper(this.AppConfiguration)).Configure(new UnityContainer());

            CoreStartupBase.Container = this._container;
            this.AppConfiguration = CoreStartupBase.AppConfiguration;

            this.Logger = this._container.Resolve<ILogger>();
            this._webApiStartOptions = new StartOptions
                                      {
                                          Port = this.AppConfiguration.WebApiPort,
                                      };

            this.ConfigureWorkers(this._container.Resolve<IWorkerManager>(), this._container);
            this.ConfigureWebServer(this._webApiStartOptions);
        }

        public bool Start(HostControl hostControl)
        {
            this.WebHost.Start();

            this.StartWorkers(this._container.Resolve<IWorkerManager>());
            this.Logger.Info(string.Join(", ", this.WebHost.ServerFeatures.Select(f => f.ToString())));
            this.Logger.Info(RuntimeHelper.GetRuntimeInfo());
            this.Logger.Info("SignalR server running on {0}", this.AppConfiguration.WebApiPort);
            this.Logger.Info("Web server running on {0}", this.AppConfiguration.WebApiPort);
            this.Logger.Info("Repository provider: {0}", this.AppConfiguration.RepositoryProvider);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }

        protected void StartWorkers(IWorkerManager workerManager)
        {
            workerManager.StartAll();
        }

        protected virtual void ConfigureWorkers(IWorkerManager workerManager, IUnityContainer container)
        {

        }

        protected virtual void ConfigureWebServer(StartOptions startOptions)
        {
            if (RuntimeHelper.IsRunningOnMono())
            {
                //webApiStartOptions.ServerFactory = "Nowin";
                this._webApiStartOptions.Urls.Add(string.Format("http://+:{0}", this._webApiStartOptions.Port));
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    this._webApiStartOptions.Urls.Add(string.Format("http://127.0.0.1:{0}", this._webApiStartOptions.Port));
                }
            }
            else
            {
                this._webApiStartOptions.Urls.Add(string.Format("http://+:{0}", this._webApiStartOptions.Port));
            }

            var webHost = new WebHostBuilder();

            if (this.AppConfiguration.WebServer == "WebListener")
            {
                webHost.UseWebListener(options => { });
            }
            else if (this.AppConfiguration.WebServer == "Kestrel")
            {
                webHost.UseKestrel(
                    options =>
                        {
                            options.MaxRequestBufferSize = 4 * 1014 * 1024;
                            options.UseConnectionLogging();
                            options.ThreadCount = this.AppConfiguration.KestrelThreads;
                        });
            }

            this.WebHost = webHost.UseStartup<TCoreStartup>().UseUrls(this._webApiStartOptions.Urls.ToArray()).Build();
        }

        protected abstract IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration);
    }
}