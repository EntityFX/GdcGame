using System.Runtime.Serialization;
using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
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