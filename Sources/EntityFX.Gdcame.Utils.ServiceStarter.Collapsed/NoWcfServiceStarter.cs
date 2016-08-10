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
    public class NoWcfServiceStarter : ServicesStarterBase<NoWcfContainerBootstrapper>, IServicesStarter, IDisposable
    {
        private readonly Uri _baseUrl = new Uri("net.pipe://localhost/");
        private readonly string _signalRHost;
        private IDisposable _webApp;
        
        public NoWcfServiceStarter(NoWcfContainerBootstrapper bootstrapper, string signalRHost) : base(bootstrapper)
        {
            _signalRHost = signalRHost;
        }

        public override void StartServices()
        {
            AddNetNamedPipeService<ISessionManager>(_baseUrl);
            AddNetNamedPipeService<ISimpleUserManager>(_baseUrl);
            AddNetNamedPipeService<IRatingManager>(_baseUrl);
            AddCustomService<GameManagerServiceHost>(_baseUrl);
            AddCustomService<AdminManagerServiceHost>(_baseUrl);

            _webApp = WebApp.Start(_signalRHost, builder =>
            {
                var listener = (HttpListener)builder.Properties[typeof(HttpListener).FullName];
                listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
                builder.UseCors(CorsOptions.AllowAll);
                builder.MapSignalR();
                builder.RunSignalR(new HubConfiguration()
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

        public void Dispose()
        {
            _webApp.Dispose();
        }
    }
}