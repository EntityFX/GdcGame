
namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /*
    public class CustomOAuthProvider : OAuthMiddleware<>
    {
        private readonly string _publicClientId;
        private readonly ISessionManagerClientFactory _sessionManager;

        public CustomOAuthProvider(string publicClientId, ISessionManagerClientFactory sessionManager)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            this._publicClientId = publicClientId;
            this._sessionManager = sessionManager;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));

            foreach (var role in user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var sessionGuid = this._sessionManager.BuildSessionManagerClient(Guid.Empty).OpenSession(context.UserName);
            identity.AddClaim(new Claim("gameSession", sessionGuid.ToString()));

            context.Validated(identity);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => context.Validated());
        }
    }*/
}