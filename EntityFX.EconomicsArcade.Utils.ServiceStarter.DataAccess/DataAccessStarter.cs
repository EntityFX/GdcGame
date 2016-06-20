using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess
{
    public class DataAccessStarter: ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {

        private const string BASE_URL = "net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/";

        public DataAccessStarter(ContainerBootstrapper container)
            :base(container)
        {

        }

        public override void StartService()
        {
            AddNetTcpService<IUserRepository>();
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
