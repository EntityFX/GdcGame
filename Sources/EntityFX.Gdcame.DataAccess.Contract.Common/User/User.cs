namespace EntityFX.Gdcame.DataAccess.Contract.Common.User
{
    using System;
    //

    //
    public class User
    {
        //
        public string Id { get; set; }

        //
        public string Login { get; set; }

        //
        public uint Role { get; set; }

        //
        public string PasswordHash { get; set; }

        public override string ToString()
        {
            return String.Format("User [Id = {0}, Login ={1}, Role={2}]",this.Id, this.Login, this.Role);
        }
    }
}