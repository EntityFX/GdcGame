using System;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class SessionManagerProxy : NetTcpProxy<ISessionManager>
    {
        private readonly Guid _sessionGuid;

        public SessionManagerProxy(Guid sessionGuid)
        {
            _sessionGuid = sessionGuid;
        }

        protected override void ApplyOperationContext()
        {
            OperationContextHelper.Instance.SessionId = _sessionGuid;
        }
    }
}