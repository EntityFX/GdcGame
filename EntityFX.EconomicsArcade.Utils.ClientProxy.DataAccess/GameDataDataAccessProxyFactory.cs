using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class GameDataDataAccessProxyFactory : NetTcpProxyFactory<IGameDataDataAccessService>
    {

        protected override void ApplyOperationContext()
        {
        }
    }
}

