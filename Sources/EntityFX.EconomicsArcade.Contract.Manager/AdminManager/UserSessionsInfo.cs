using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Contract.Manager.AdminManager
{
    [DataContract]
    public class UserSessionsInfo
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public Session[] UserSessions { get; set; }
    }
}
