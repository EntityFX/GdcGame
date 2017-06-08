using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.User
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public uint Role { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        public override string ToString()
        {
            return String.Format("User [Id = {0}, Login ={1}, Role={2}]",Id, Login, Role);
        }
    }
}