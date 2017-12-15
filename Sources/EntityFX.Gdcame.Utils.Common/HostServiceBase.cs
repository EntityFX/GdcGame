using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EntityFX.Gdcame.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

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
    using Autofac.Core;

    //using Topshelf;

    public abstract class HostServiceBase<TCoreStartup>
        where TCoreStartup : CoreStartupBase
    {
        private readonly IRuntimeHelper _runtimeHelper;

        private readonly AppConfiguration _appConfiguration;

        //private readonly StartOptions _webApiStartOptions;

        protected ILogger Logger;

        protected IWebHost WebHost;

        private IIocContainer _container;

        public HostServiceBase(IRuntimeHelper runtimeHelper, AppConfiguration appConfiguration, IIocContainer container)
        {
            _runtimeHelper = runtimeHelper;
            _appConfiguration = appConfiguration;
            _container = container;

            //this.ConfigureWebServer(this._webApiStartOptions);
        }

        public bool Start()
        {
           this._container = Configure(_container);

            CoreStartupBase.AppConfiguration = this._appConfiguration;
            CoreStartupBase.IocContainer = _container;
            CoreStartupBase.PrepareContainer += CoreStartupBase_PrepareContainer;
            CoreStartupBase.LoadedAssemblies = _runtimeHelper.GetLoadedAssemblies().ToArray();
            //(_container as IIocContainer<Autofac.ContainerBuilder, IContainer>).Configure();
            this.WebHost = ConfigureWebServer();

           this.Logger = this._container.Resolve<ILogger>();


            this.ConfigureWorkers(this._container.Resolve<IWorkerManager>(), this._container);


            //this.WebHost.Start();

            this.StartWorkers(this._container.Resolve<IWorkerManager>());
            this.Logger.Info(string.Join(", ", this.WebHost.ServerFeatures.Select(f => f.ToString())));
            this.Logger.Info(_runtimeHelper.GetRuntimeInfo());
            this.Logger.Info("Web server running on {0}", this._appConfiguration.WebApiPort);
            this.Logger.Info("Repository provider: {0}", this._appConfiguration.RepositoryProvider);

            this.WebHost.Run();
            return true;
        }

        private IServiceProvider CoreStartupBase_PrepareContainer(IIocContainer arg, IServiceCollection serviceCollection)
        {
            var containerBuilder = (arg as IIocContainer<ContainerBuilder, IContainer>);
            containerBuilder.ContainerBuilder.Populate(serviceCollection);
            var container = containerBuilder.Configure();
            return new AutofacServiceProvider(container);
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

        protected virtual IWebHost ConfigureWebServer()
        {
            //var webHost = new WebHostBuilder()
            //    .UseKestrel(
            //        options =>
            //        {
            //            options.UseConnectionLogging();
            //            options.ThreadCount = 32;
            //            options.Limits.MaxRequestBufferSize = 4 * 1014 * 1024;
            //        })
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseStartup<TCoreStartup>().UseUrls(new[] { "http://+:9999" })
            //    .UseEnvironment("Development")
            //    .Build();

            var urls = new List<string>();
            if (_runtimeHelper.IsRunningOnMono())
            {
                //webApiStartOptions.ServerFactory = "Nowin";
                //urls.Add(string.Format("http://+:{0}", this._appConfiguration.WebApiPort));
                //if (Environment.OSVersion.Platform != PlatformID.Unix)
                //{
                //    this._webApiStartOptions.Urls.Add(string.Format("http://127.0.0.1:{0}", this._webApiStartOptions.Port));
                //}
            }
            else
            {
                urls.Add(string.Format("http://+:{0}", this._appConfiguration.WebApiPort ));
            }

            IWebHostBuilder webHost = new WebHostBuilder();
               // .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
               // .ConfigureServices(services => services.AddAutofac());
            if (_appConfiguration.WebServer == "WebListener")
            {
                webHost = webHost.UseWebListener(options => { });
            }
            else if (_appConfiguration.WebServer == "Kestrel")
            {
                webHost = webHost.UseKestrel(
                    options =>
                        {
                            //options.UseConnectionLogging();
                            //options.ThreadCount = _appConfiguration.KestrelThreads;
                            options.Limits.MaxRequestBufferSize = 4 * 1014 * 1024;
                        });
            }

           return webHost.UseStartup<TCoreStartup>().UseUrls(urls.ToArray()).Build();
            //return webHost;
        }

        protected abstract IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration, IIocContainer container);

        protected IIocContainer Configure(IIocContainer container)
        {
            container = (this.GetContainerBootstrapper(this._appConfiguration, _container)).Configure(_container);
            return ConfigureAdvanced(container);
        }

        protected virtual IIocContainer ConfigureAdvanced(IIocContainer container)
        {
            return container;
        }
    }
}