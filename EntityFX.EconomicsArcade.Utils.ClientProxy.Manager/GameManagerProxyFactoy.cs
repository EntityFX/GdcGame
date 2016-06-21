using System;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class GameManagerProxyFactoy : NetTcpProxyFactory<IGameManager>
    {
        private readonly Guid _sessionGuid;

        public GameManagerProxyFactoy(Guid sessionGuid)
        {
            _sessionGuid = sessionGuid;
        }

        protected override void ApplyOperationContext()
        {
            OperationContextHelper.Instance.SessionId = _sessionGuid;
        }
    }
}