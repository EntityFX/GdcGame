using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using System;
using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Manager;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    public class ManagerStarter: ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {
        private readonly Uri _baseUrl;// = "net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/";

        public ManagerStarter(ContainerBootstrapper container, string baseUrl)
            :base(container)
        {
            _baseUrl = new Uri(baseUrl);
        }

        public override void StartService()
        {
            AddNetTcpService<ISessionManager>(_baseUrl);
            AddNetTcpService<ISimpleUserManager>(_baseUrl);
            AddNetTcpService<IAdminManager>(_baseUrl);
            AddNetTcpService<IRatingManager>(_baseUrl);
            AddCustomService<GameManagerServiceHost>(_baseUrl);
            OpenServices();
        }

        public override void StopService()
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
    }
}
