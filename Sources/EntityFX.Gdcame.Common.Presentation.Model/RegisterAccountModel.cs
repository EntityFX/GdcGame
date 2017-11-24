//using System.Web.Mvc;


namespace EntityFX.Gdcame.Common.Application.Model
{
    public class RegisterAccountModel
    {
        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual string ConfirmPassword { get; set; }
    }
}