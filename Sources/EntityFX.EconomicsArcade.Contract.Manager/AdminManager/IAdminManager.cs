using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Manager.AdminManager
{
    public interface IAdminManager
    {
        UserSessionsInfo[] GetActiveSessions();

        void CloseSessionByGuid(Guid guid);
        void CloseAllUserSessions(string username);

        void WipeUser(string username);
    }
}
