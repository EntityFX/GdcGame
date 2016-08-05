using System;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ServiceStarter.DataAccess
{
    public class DataAccessStarter: ServicesStarterBase<ContainerBootstrapper>, IServicesStarter
    {

        private readonly Uri _baseUrl;// = "net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/";

        private readonly Uri _baseStoreUrl;// = "net.msmq://localhost/private/";

        public DataAccessStarter(ContainerBootstrapper container, string baseUrl, string baseStoreUrl)
            :base(container)
        {
            _baseUrl = new Uri(baseUrl);
            _baseStoreUrl = new Uri(baseStoreUrl);
        }

        public override void StartServices()
        {
            AddNetTcpService<IUserDataAccessService>(_baseUrl);
            AddNetTcpService<IGameDataRetrieveDataAccessService>(_baseUrl);
            AddNetMsmqService<IGameDataStoreDataAccessService>(_baseStoreUrl);
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
    }
}
