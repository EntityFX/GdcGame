//

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    //
    public class UserSessionsInfo
    {
        //
        public string UserName { get; set; }

        //
        public Session[] UserSessions { get; set; }
    }
}