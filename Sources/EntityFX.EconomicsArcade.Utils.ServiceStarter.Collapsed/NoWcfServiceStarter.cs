using System;
using System.Net;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Collapsed
{
    public class NoWcfServiceStarter : ServicesStarterBase<NoWcfContainerBootstrapper>, IServicesStarter, IDisposable
    {
        private readonly Uri _baseUrl = new Uri("net.pipe://localhost/");
        private readonly string _signalRHost = "http://+:8080";
        private IDisposable _webApp;
        
        public NoWcfServiceStarter(NoWcfContainerBootstrapper bootstrapper) : base(bootstrapper)
        {
        }

        public override void StartServices()
        {
            AddNetNamedPipeService<ISessionManager>(_baseUrl);
            AddNetNamedPipeService<ISimpleUserManager>(_baseUrl);
            AddNetNamedPipeService<IRatingManager>(_baseUrl);
            AddCustomService<GameManagerServiceHost>(_baseUrl);
            AddCustomService<AdminManagerServiceHost>(_baseUrl);
            AddNetNamedPipeService<INotifyConsumerService>(_baseUrl);

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