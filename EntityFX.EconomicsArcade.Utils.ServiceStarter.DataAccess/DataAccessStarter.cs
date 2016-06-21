using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using System;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess
{
    public class DataAccessStarter: ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {

        private const string BaseUrl = "net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/";

        public DataAccessStarter(ContainerBootstrapper container)
            :base(container)
        {

        }

        public override void StartService()
        {
            AddNetTcpService<IUserDataAccessService>();
            AddNetTcpService<IGameDataDataAccessService>();
            OpenServices(new Uri(BaseUrl));
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
