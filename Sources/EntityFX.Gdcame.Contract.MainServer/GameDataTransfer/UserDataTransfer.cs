using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Contract.MainServer.GameDataTransfer
{
    [DataContract]
    public class UserDataTransfer
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
            return String.Format("User [Id = {0}, Login ={1}, Role={2}]", this.Id, this.Login, this.Role);
        }
    }
}
