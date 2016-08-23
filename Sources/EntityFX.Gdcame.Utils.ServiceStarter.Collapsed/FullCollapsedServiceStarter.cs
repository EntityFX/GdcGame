using System;
using System.Net;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Collapsed
{
    public class FullCollapsedServiceStarter : ServicesStarterBase<FullCollapsedContainerBootstrapper>, IServicesStarter,
        IDisposable
    {
        private readonly string _baseUrl = "net.tcp://localhost";
        private readonly string _signalRHost;
        private IDisposable _webApp;

        public FullCollapsedServiceStarter(FullCollapsedContainerBootstrapper bootstrapper, string signalRHost)
            : base(bootstrapper)
        {
            _signalRHost = signalRHost;
        }

        public void Dispose()
        {
            _webApp.Dispose();
        }

        public override void StartServices()
        {
            AddNetTcpService<ISessionManager>(new Uri(_baseUrl + ":10000/"));
            AddNetTcpService<ISimpleUserManager>(new Uri(_baseUrl + ":10001/"));
            AddNetTcpService<IRatingManager>(new Uri(_baseUrl + ":10002/"));
            AddCustomService<GameManagerTcpServiceHost>(new Uri(_baseUrl + ":10003/"));
            AddCustomService<AdminManagerTcpServiceHost>(new Uri(_baseUrl + ":10004/"));

            _webApp = WebApp.Start(_signalRHost, builder =>
            {
                var listener = (HttpListener) builder.Properties[typeof (HttpListener).FullName];
                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                builder.UseCors(CorsOptions.AllowAll);
                builder.MapSignalR();
                builder.RunSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJSONP = true
                });
            });
            Console.WriteLine("Server running on {0}", _signalRHost);
            OpenServices();
        }

        public override void StopServices()
        {
            CloseServices();
        }

        protected override void OnServiceOpened(IServiceHost service)
        {
            var serviceInfoHelper = _container.Resolve<IServiceInfoHelper>();
            serviceInfoHelper.PrintServiceHostInfo(service.ServiceHost);
            //ServiceInfoHelperConsole.PrintServiceHostInfo(service.ServiceHost);
        }
    }
}