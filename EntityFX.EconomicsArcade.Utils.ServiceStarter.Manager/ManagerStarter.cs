using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using System;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;

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
            AddNetTcpService<IGameManager>(_baseUrl);
            AddNetTcpService<ISimpleUserManager>(_baseUrl);
            OpenServices();
        }

        public override void StopService()
        {
            CloseServices();
        }

        protected override void OnServiceOpened(IServiceHost service)
        {
            ServiceInfoHelper.PrintServiceHostInfo(service.ServiceHost);
        }
    }
}
