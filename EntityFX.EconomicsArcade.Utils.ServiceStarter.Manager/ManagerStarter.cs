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
        private const string BaseUrl = "net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/";

        public ManagerStarter(ContainerBootstrapper container)
            :base(container)
        {

        }

        public override void StartService()
        {
            AddNetTcpService<ISessionManager>(new Uri(BaseUrl));
            AddNetTcpService<IGameManager>(new Uri(BaseUrl));
            AddNetTcpService<ISimpleUserManager>(new Uri(BaseUrl));
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
