﻿
using System.ComponentModel.DataAnnotations;

namespace EntityFX.Gdcame.Application.Contract.Model.MainServer
{
    public class RegisterAccountModel : Common.Application.Model.RegisterAccountModel
    {
        [Required]
        //[Display(Name = "Login")]
        public override string Login { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        public override string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public override string ConfirmPassword { get; set; }
    }
}
