using System.Runtime.Serialization;
using EntityFX.Gdcame.Manager.Contract.SessionManager;

namespace EntityFX.Gdcame.Manager.Contract.AdminManager
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
