﻿using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ServiceStarter.WcfCollapsed
{
    internal class AdminManagerTcpServiceHost : NetTcpServiceHost<IAdminManager>
    {
        public AdminManagerTcpServiceHost(IUnityContainer container)
            : base(container)
        {
        }

        protected override void BeforeServiceOpen(ServiceHost serviceHost)
        {
            var serviceEndpointCollection = serviceHost.Description.Endpoints;
            foreach (var serviceEndpoint in serviceEndpointCollection)
            {
                foreach (var operation in serviceEndpoint.Contract.Operations)
                {
                    operation.Behaviors.Add(
                        new CheckRolePermissionsOperationBehavior(Container.Resolve<IOperationContextHelper>(),
                            Container.Resolve<GameSessions>()));
                }
            }
        }
    }
}