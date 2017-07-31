using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EntityFX.Gdcame.Application.Api.MainServer.Models;
using EntityFX.Gdcame.Application.Api.MainServer.Providers;
using EntityFX.Gdcame.Infrastructure.Api.ApiResult;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using RegisterAccountModel = EntityFX.Gdcame.Application.Api.MainServer.Models.RegisterAccountModel;

namespace EntityFX.Gdcame.Application.Api.MainServer.Controllers
{
    using EntityFX.Gdcame.Application.Api.Common;
    using EntityFX.Gdcame.Application.Api.Common.Providers;
    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;

    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        private readonly ISessionManager _sessionManager;
        private const string LocalLoginProvider = "Local";
        private UserManager<UserIdentity> _userManager;

        public UserManager<UserIdentity> UserManager
        {
            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public AuthController(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }


        // GET api/Account/UserInfo
        public UserInfoViewModel GetUserInfo()
        {
            return new UserInfoViewModel
            {
                Email = RequestContext.Principal.Identity.GetUserName()
            };
        }

        // POST api/Auth/Logout
        [Route("Logout")]
        public bool Logout()
        {
            _sessionManager.CloseSession();
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return true;
        }


        // POST api/Auth/Register
        [Route("Register")]
        [HttpPost]
        public async Task<HttpResponseMessage> Register(RegisterAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var user = new UserIdentity { UserName = model.Login };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
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

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private HttpResponseMessage GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Cannot register");
            }

            if (!result.Succeeded)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiErrorResult()
                {
                    Message = "Cannot register",
                    ErrorDetails = result.Errors.Select(error => new ApiErrorData()
                    {
                        Code = 0,
                        Message = error
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
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                var strengthInBytes = strengthInBits / bitsPerByte;

                var data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}