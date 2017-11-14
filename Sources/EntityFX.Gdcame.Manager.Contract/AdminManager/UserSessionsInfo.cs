//

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common;

    //
    public class UserSessionsInfo
    {
        //
        public string UserName { get; set; }

        //
        public Session[] UserSessions { get; set; }
    }
}