using System;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class UserRepositoryProxyFactoy : NetTcpProxyFactory<IUserRepository>
    {

        protected override void ApplyOperationContext()
        {
        }
    }
}