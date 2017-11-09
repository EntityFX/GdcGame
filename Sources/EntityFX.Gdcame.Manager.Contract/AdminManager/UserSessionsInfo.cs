//using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    //[DataContract]
    public class UserSessionsInfo
    {
        //[DataMember]
        public string UserName { get; set; }

        //[DataMember]
        public Session[] UserSessions { get; set; }
    }
}