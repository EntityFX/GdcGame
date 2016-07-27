using System;
using System.Net;
using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using EntityFX.EconomicsArcade.Infrastructure.Service.NetNamedPipe;
using EntityFX.EconomicsArcade.Manager;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Owin;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Collapsed
{
    public class CollapsedServiceStarter : ServicesStarterBase<ContainerBootstrapper>, IServicesStarter, IDisposable
    {
        private readonly Uri _baseUrl = new Uri("net.pipe://localhost/");
        private readonly Uri _baseMsmqUrl = new Uri("net.msmq://localhost/private/");
        private readonly string _signalRHost = "http://+:8080";
        private IDisposable _webApp;

        public CollapsedServiceStarter(ContainerBootstrapper bootstrapper)
            : base(bootstrapper)
        {
        }

        public override void StartServices()
        {
            AddNetNamedPipeService<IUserDataAccessService>(_baseUrl);
            AddNetNamedPipeService<IGameDataRetrieveDataAccessService>(_baseUrl);
            AddNetMsmqService<IGameDataStoreDataAccessService>(_baseMsmqUrl);

            AddNetNamedPipeService<ISessionManager>(_baseUrl);
            AddNetNamedPipeService<ISimpleUserManager>(_baseUrl);
            AddNetNamedPipeService<IRatingManager>(_baseUrl);
            AddCustomService<GameManagerServiceHost>(_baseUrl);
            AddCustomService<AdminManagerServiceHost>(_baseUrl);

            AddNetMsmqService<INotifyConsumerService>(_baseMsmqUrl);

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

        private class GameManagerServiceHost : NetNamedPipeServiceHost<IGameManager>
        {
            public GameManagerServiceHost(IUnityContainer container)
                : base(container)
            {
            }

            protected override void BeforeServiceOpen(ServiceHost serviceHost)
            {
                serviceHost.Description.Behaviors.Add(new ErrorHandlerBehavior(new InvalidSessionFaultHandler()));
            }
        }

        private class AdminManagerServiceHost : NetNamedPipeServiceHost<IAdminManager>
        {
            public AdminManagerServiceHost(IUnityContainer container)
                : base(container)
            {
            }

            protected override void BeforeServiceOpen(ServiceHost serviceHost)
            {
                var serviceEndpointCollection = serviceHost.Description.Endpoints;
                foreach (var serviceEndpoint in serviceEndpointCollection)
                {
                    foreach (var operation in serviceEndpoint.Contract.Operations)
                    {
                        operation.Behaviors.Add(new CheckRolePermissionsOperationBehavior(Container.Resolve<GameSessions>()));
                    }
                }
            }
        }

    }
}