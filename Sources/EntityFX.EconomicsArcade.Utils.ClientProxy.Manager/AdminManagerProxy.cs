using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class AdminManagerProxy : NetTcpProxy<IAdminManager>
    {
        private Guid _sessionGuid;

        public AdminManagerProxy(Guid sessionGuid)
        {
            _sessionGuid = sessionGuid;
        }

        protected override void ApplyOperationContext()
        {
            OperationContextHelper.Instance.SessionId = _sessionGuid;
        }
    }
}
