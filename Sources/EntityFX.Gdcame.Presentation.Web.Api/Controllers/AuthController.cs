using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Api.MainServer.Models;
using EntityFX.Gdcame.Infrastructure.Api.ApiResult;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegisterAccountModel = EntityFX.Gdcame.Application.Api.MainServer.Models.RegisterAccountModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace EntityFX.Gdcame.Application.Api.MainServer.Controllers
{
    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;

    public class IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ISessionManager _sessionManager;
        private const string LocalLoginProvider = "Local";
        private UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public AuthController(ISessionManager sessionManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _sessionManager = sessionManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // GET api/Account/UserInfo
        public UserInfoViewModel GetUserInfo()
        {
            return new UserInfoViewModel
            {
                Email = HttpContext.User.Identity.Name
            };
        }

        // POST api/Auth/Logout
        [Route("Logout")]
        public async Task<bool> Logout()
        {
            _sessionManager.CloseSession();
            await _signInManager.SignOutAsync();
            return true;
        }


        // POST api/Auth/Register
        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser() { UserName = model.Login };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers


        private ActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return StatusCode(500, "Cannot register");
            }

            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResult()
                {
                    Message = "Cannot register",
                    ErrorDetails = result.Errors.Select(error => new ApiErrorData()
                    {
                        Code = 0,
                        Message = error.Description
                    }).ToArray()
                });
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || string.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || string.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.Name
                };
            }
        }

        #endregion
    }
}