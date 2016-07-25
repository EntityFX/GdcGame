using System.Runtime.Serialization;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Contract.Manager.AdminManager
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