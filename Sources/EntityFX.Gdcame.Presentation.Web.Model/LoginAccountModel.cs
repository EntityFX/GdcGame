using System.ComponentModel.DataAnnotations;

namespace EntityFX.Gdcame.Application.Contract.Model.MainServer
{
    public class LoginAccountModel : Common.Application.Model.LoginAccountModel
    {
        [Required]
        public override string Login { get; set; }
        [Required]
        public override string Password { get; set; }
    }
}