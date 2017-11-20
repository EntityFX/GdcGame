using System.Collections.Generic;
using EntityFX.Gdcame.Infrastructure;

namespace EntityFX.Gdcame.Utils.Common
{
    using System;
    using System.Linq;
    using System.Threading;

    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    using Microsoft.AspNetCore.Hosting;
    //using Microsoft.Owin.Hosting;
    using Unity;

    //using Topshelf;

    public abstract class HostServiceBase<TCoreStartup>
        where TCoreStartup : CoreStartupBase
    {
        private readonly IRuntimeHelper _runtimeHelper;

        private readonly IServiceProvider _serviceProvider;
        private readonly AppConfiguration _appConfiguration;

        //private readonly StartOptions _webApiStartOptions;

        protected readonly ILogger Logger;

        protected IWebHost WebHost;

        private IIocContainer _container;

        public HostServiceBase(IRuntimeHelper runtimeHelper, IIocContainer iocContainer, IServiceProvider serviceProvider, AppConfiguration appConfiguration)
        {
            _runtimeHelper = runtimeHelper;
            _serviceProvider = serviceProvider;
            _appConfiguration = appConfiguration;

            CoreStartupBase.AppConfiguration = this._appConfiguration;
            this._container = (this.GetContainerBootstrapper(this._appConfiguration)).Configure(iocContainer);

            CoreStartupBase.ServiceProvider = _serviceProvider;
 
            this.Logger = this._container.Resolve<ILogger>();


            this.ConfigureWorkers(this._container.Resolve<IWorkerManager>(), this._container);
            //this.ConfigureWebServer(this._webApiStartOptions);
        }

        public IIocContainer Container
        {
            get { return _container; }
        }

        public bool Start()
        {
            this.WebHost.Start();

            this.StartWorkers(this._container.Resolve<IWorkerManager>());
            this.Logger.Info(string.Join(", ", this.WebHost.ServerFeatures.Select(f => f.ToString())));
            this.Logger.Info(_runtimeHelper.GetRuntimeInfo());
            this.Logger.Info("Web server running on {0}", this._appConfiguration.WebApiPort);
            this.Logger.Info("Repository provider: {0}", this._appConfiguration.RepositoryProvider);
            return true;
        }

        public bool Stop()
        {
            return true;
        }

        protected void StartWorkers(IWorkerManager workerManager)
        {
            workerManager.StartAll();
        }

        protected virtual void ConfigureWorkers(IWorkerManager workerManager, IIocContainer container)
        {

        }

        protected virtual void ConfigureWebServer()
        {
            var urls = new List<string>();
            if (_runtimeHelper.IsRunningOnMono())
            {
                //webApiStartOptions.ServerFactory = "Nowin";
                urls.Add(string.Format("http://+:{0}", this._appConfiguration.WebApiPort));
              /*  if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    this._webApiStartOptions.Urls.Add(string.Format("http://127.0.0.1:{0}", this._webApiStartOptions.Port));
                }*/
            }
            else
            {
                urls.Add(string.Format("http://+:{0}", this._appConfiguration.WebApiPort));
            }

            var webHost = new WebHostBuilder();

            if (_appConfiguration.WebServer == "WebListener")
            {
                webHost.UseWebListener(options => { });
            }
            else if (_appConfiguration.WebServer == "Kestrel")
            {
                webHost.UseKestrel(
                    options =>
                        {
                            options.UseConnectionLogging();
                            options.ThreadCount = _appConfiguration.KestrelThreads;
                            options.Limits.MaxRequestBufferSize = 4 * 1014 * 1024;
                        });
            }

            this.WebHost = webHost.UseStartup<TCoreStartup>().UseUrls(urls.ToArray()).Build();

        }

        protected abstract IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration);
    }
}