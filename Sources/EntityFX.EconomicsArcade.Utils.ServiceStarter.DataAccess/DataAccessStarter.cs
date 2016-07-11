using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using System;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess
{
    public class DataAccessStarter: ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {

        private readonly Uri _baseUrl;// = "net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/";

        private readonly Uri _baseStoreUrl;// = "net.msmq://localhost/private/";

        public DataAccessStarter(ContainerBootstrapper container, string baseUrl, string baseStoreUrl)
            :base(container)
        {
            _baseUrl = new Uri(baseUrl);
            _baseStoreUrl = new Uri(baseStoreUrl);
        }

        public override void StartService()
        {
            AddNetTcpService<IUserDataAccessService>(_baseUrl);
            AddNetTcpService<IGameDataRetrieveDataAccessService>(_baseUrl);
            AddNetMsmqService<IGameDataStoreDataAccessService>(_baseStoreUrl);
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
