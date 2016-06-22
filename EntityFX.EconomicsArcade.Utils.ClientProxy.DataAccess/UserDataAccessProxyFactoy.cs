using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class UserDataAccessProxyFactoy : NetTcpProxy<IUserDataAccessService>
    {

        protected override void ApplyOperationContext()
        {
        }
    }
}