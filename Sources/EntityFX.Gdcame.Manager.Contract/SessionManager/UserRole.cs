using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager
{
    [DataContract]
    public enum UserRole
    {
        [EnumMember] GenericUser,
        [EnumMember] Admin,
        [EnumMember] System
    }
}