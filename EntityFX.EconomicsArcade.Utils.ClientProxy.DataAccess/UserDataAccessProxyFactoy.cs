using System;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class UserDataAccessProxyFactoy : NetTcpProxyFactory<IUserDataAccessService>
    {

        protected override void ApplyOperationContext()
        {
        }
    }
}