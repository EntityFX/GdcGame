using System.Runtime.Serialization;
using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    [DataContract]
    public class InsufficientPermissionsFault
    {
        [DataMember]
        public UserRole[] RequiredRoles { get; set; }

        [DataMember]
        public UserRole[] CurrentRoles { get; set; }
    }
}