using System;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class GameManagerProxy : NetTcpProxy<IGameManager>
    {
        private readonly Guid _sessionGuid;

        public GameManagerProxy(Guid sessionGuid)
        {
            _sessionGuid = sessionGuid;
        }

        protected override void ApplyOperationContext()
        {
            OperationContextHelper.Instance.SessionId = _sessionGuid;
        }
    }
}