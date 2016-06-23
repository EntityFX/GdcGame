using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using System;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess
{
    public class DataAccessStarter: ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {

        private const string BaseUrl = "net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/";

        private const string BaseStoreUrl = "net.msmq://localhost/private/StoreGameData";

        public DataAccessStarter(ContainerBootstrapper container)
            :base(container)
        {

        }

        public override void StartService()
        {
            AddNetTcpService<IUserDataAccessService>(new Uri(BaseUrl));
            AddNetTcpService<IGameDataRetrieveDataAccessService>(new Uri(BaseUrl));
            AddNetMsmqService<IGameDataStoreDataAccessService>(new Uri(BaseStoreUrl));
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
