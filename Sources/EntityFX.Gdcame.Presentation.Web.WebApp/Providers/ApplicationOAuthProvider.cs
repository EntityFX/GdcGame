using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EntityFX.Gdcame.Presentation.Web.WebApp.Models;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;

namespace EntityFX.Gdcame.Presentation.Web.WebApp.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly ISessionManagerClientFactory _sessionManager;

        public ApplicationOAuthProvider(string publicClientId, ISessionManagerClientFactory sessionManager)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
            _sessionManager = sessionManager;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            GameUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            //ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);
            //ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);


            //AuthenticationProperties properties = CreateProperties(user.UserName);
            //AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            var sessionGuid = _sessionManager.BuildSessionManagerClient(Guid.Empty).OpenSession(context.UserName);
            identity.AddClaim(new Claim("gameSession", sessionGuid.ToString()));

            context.Validated(identity);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

    }
}