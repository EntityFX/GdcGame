using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    public class ManagerStarter: ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {
        InfrastructureServiceHost<ISessionManager> _sessionManager;

        private const string BASE_URL = "net.tcp://localhost/EntityFX.EconomicsArcade.Manager/";

        public ManagerStarter(ContainerBootstrapper container)
            :base(container)
        {

        }

        public override void StartService()
        {
            AddNetTcpService<ISessionManager>();
            OpenServices(new Uri(BASE_URL));
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
