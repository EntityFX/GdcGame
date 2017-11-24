using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Api.Common.Providers;
using EntityFX.Gdcame.Application.Api.MainServer.Models;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Api.ApiResult;
using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using LoginAccountModel = EntityFX.Gdcame.Common.Application.Model.LoginAccountModel;
using RegisterAccountModel = EntityFX.Gdcame.Application.Contract.Model.MainServer.RegisterAccountModel;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    [Route("api/[controller]")]
    public class AuthController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ISessionManager _sessionManager;
        private const string LocalLoginProvider = "Local";
        private UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;


        public AuthController(ISessionManager sessionManager, UserManager<UserIdentity> userManager, SignInManager<UserIdentity> signInManager)
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

            var user = new UserIdentity() { UserName = model.Login };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken(LoginAccountModel loginAccount)
        {
            var signIResult =
                await _signInManager.PasswordSignInAsync(loginAccount.Login, loginAccount.Password, true, false);

            if (signIResult == null || !signIResult.Succeeded)
            {
                return BadRequest("Invalid user name or password");
            }

            var user = await _userManager.FindByNameAsync(loginAccount.Login);
            var roles = await _userManager.GetRolesAsync(user);
            var options = new JwtIssuerOptions();

            var handler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var defaultClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            };
            defaultClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var guid = _sessionManager.OpenSession(user.UserName);
            defaultClaims.Add(new Claim("game-session", guid.ToString()));

            var identity = new ClaimsIdentity(defaultClaims.ToArray());
            var token = handler.CreateJwtSecurityToken(subject: identity,
                signingCredentials: signingCredentials,
                audience: options.Audience,
                issuer: options.Issuer,
                expires: options.Expiration);
            return new JsonResult(new {access_token = new JwtSecurityTokenHandler().WriteToken(token)});
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