using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;


namespace EntityFX.Gdcame.Application.Contract.Model
{
    public class RegisterAccountModel
    {
        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual string ConfirmPassword { get; set; }
    }
}