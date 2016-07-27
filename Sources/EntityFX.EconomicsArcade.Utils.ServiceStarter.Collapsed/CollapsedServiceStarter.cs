using System;
using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using EntityFX.EconomicsArcade.Infrastructure.Service.NetNamedPipe;
using EntityFX.EconomicsArcade.Manager;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Collapsed
{
    public class CollapsedServiceStarter : ServicesStarterBase<ContainerBootstrapper>, IServicesStarter
    {
        private readonly ContainerBootstrapper _bootstrapper;
        private readonly Uri _baseUrl = new Uri("net.pipe://localhost/");
        private readonly Uri _baseStoreUrl = new Uri("net.msmq://localhost/private/");

        public CollapsedServiceStarter(ContainerBootstrapper bootstrapper) : base(bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        public override void StartServices()
        {
            AddNetNamedPipeService<IUserDataAccessService>(_baseUrl);
            AddNetNamedPipeService<IGameDataRetrieveDataAccessService>(_baseUrl);
            AddNetMsmqService<IGameDataStoreDataAccessService>(_baseStoreUrl);

            AddNetNamedPipeService<ISessionManager>(_baseUrl);
            AddNetNamedPipeService<ISimpleUserManager>(_baseUrl);
            AddNetNamedPipeService<IRatingManager>(_baseUrl);
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
            //ServiceInfoHelperConsole.PrintServiceHostInfo(service.ServiceHost);

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