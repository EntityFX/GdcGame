﻿using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess
{
    public class DataAccessStarter: ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {

        private const string BASE_URL = "net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/";

        public DataAccessStarter(ContainerBootstrapper container)
            :base(container)
        {

        }

        public override void StartService()
        {
            AddNetTcpService<IUserRepository>();
            AddNetTcpService<IGameDataRepository>();
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
