using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityFX.Gdcame.Contract.MainServer.GameDataTransfer
{
    
    public class UserDataTransfer
    {
        
        public string Id { get; set; }

        
        public string Login { get; set; }

        
        public uint Role { get; set; }

        
        public string PasswordHash { get; set; }

        public override string ToString()
        {
            return String.Format("User [Id = {0}, Login ={1}, Role={2}]", this.Id, this.Login, this.Role);
        }
    }
}
