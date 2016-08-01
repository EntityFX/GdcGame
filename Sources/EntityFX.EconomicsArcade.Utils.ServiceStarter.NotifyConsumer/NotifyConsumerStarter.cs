using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer
{
    public class NotifyConsumerStarter : ServicesStarterBase<ContainerBootstrapper>, IServicesStarter, IDisposable
    {
        private readonly string _signalRHost;
        private readonly Uri _baseUrl;
        private IDisposable _webApp;

        public NotifyConsumerStarter(ContainerBootstrapper containerBootstrapper, string baseUrl, string signalRHost)
            : base(containerBootstrapper)
        {
            _signalRHost = signalRHost;
            _baseUrl = new Uri(baseUrl);
        }

        public override void StartServices()
        {
            AddCustomService<NotifyConsumerServiceHost>(_baseUrl);
            OpenServices();

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

        }

        public override void StopServices()
        {
            CloseServices();
        }

        protected override void OnServiceOpened(IServiceHost service)
        {
            var serviceInfoHelper = _container.Resolve<IServiceInfoHelper>();
            serviceInfoHelper.PrintServiceHostInfo(service.ServiceHost);
        }

        public void Dispose()
        {
            _webApp.Dispose();
        }

        private class NotifyConsumerServiceHost : NetMsmqServiceHost<INotifyConsumerService>
        {
            public NotifyConsumerServiceHost(IUnityContainer container) : base(container)
            {
            }

            protected override Binding GetBinding()
            {
                var binding = (NetMsmqBinding)base.GetBinding();
                binding.ExactlyOnce = false;
                binding.Durable = false;
                return binding;
            }
        }
    }
}
