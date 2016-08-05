using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.SessionManager
{
    [DataContract]
    public enum UserRole
    {
        [EnumMember]
        GenericUser,
        [EnumMember]
        Admin
    }
}
