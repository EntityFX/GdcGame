namespace EntityFX.Gdcame.Contract.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public class UserData
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        [DataMember]
        public UserRole[] UserRoles { get; set; }
    }
}