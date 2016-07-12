using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Contract.Manager.AdminManager
{
    public class UserSessionsInfo
    {
        public string UserName { get; set; }
        public Session[] UserSessions { get; set; }
    }
}
