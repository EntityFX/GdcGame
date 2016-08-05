using System;
using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Manager
{
    public class ManagerStarter: ServicesStarterBase<ContainerBootstrapper>, IServicesStarter
    {
        private readonly Uri _baseUrl;// = "net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/";

        public ManagerStarter(ContainerBootstrapper container, string baseUrl)
            :base(container)
        {
            _baseUrl = new Uri(baseUrl);
        }

        public override void StartServices()
        {
            AddNetTcpService<ISessionManager>(_baseUrl);
            AddNetTcpService<ISimpleUserManager>(_baseUrl);
            AddNetTcpService<IRatingManager>(_baseUrl);
            AddCustomService<GameManagerServiceHost>(_baseUrl);
            AddCustomService<AdminManagerServiceHost>(_baseUrl);
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
        }

        private class GameManagerServiceHost : NetTcpServiceHost<IGameManager>
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

        private class AdminManagerServiceHost : NetTcpServiceHost<IAdminManager>
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
                        operation.Behaviors.Add(new CheckRolePermissionsOperationBehavior(Container.Resolve<IOperationContextHelper>(), Container.Resolve<GameSessions>()));
                    }
                }
            }
        }
    }
}
