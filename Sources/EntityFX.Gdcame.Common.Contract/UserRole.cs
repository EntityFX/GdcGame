namespace EntityFX.Gdcame.Contract.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum UserRole
    {
        [EnumMember] GenericUser,
        [EnumMember] Admin,
        [EnumMember] System
    }
}