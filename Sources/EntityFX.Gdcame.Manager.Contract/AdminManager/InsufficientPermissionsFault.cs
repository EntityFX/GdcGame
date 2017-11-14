//

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common;

    //
    public class InsufficientPermissionsFault
    {
        //
        public UserRole[] RequiredRoles { get; set; }

        //
        public UserRole[] CurrentRoles { get; set; }
    }
}