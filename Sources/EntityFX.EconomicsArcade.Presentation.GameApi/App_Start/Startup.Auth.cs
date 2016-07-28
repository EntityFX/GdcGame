using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Presentation.GameApi.EntityFX.EconomicsArcade.Presentation.GameApi;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Owin;
using EntityFX.EconomicsArcade.Presentation.GameApi.Providers;
using EntityFX.EconomicsArcade.Presentation.GameApi.Models;
using Thinktecture.IdentityModel.Owin;
using Unity.WebApi;

namespace EntityFX.EconomicsArcade.Presentation.GameApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            var unityResolver = GlobalConfiguration.Configuration.DependencyResolver as UnityDependencyResolver;
            var aumf = (ApplicationUserManagerFacrtory)unityResolver.GetService(typeof (ApplicationUserManagerFacrtory));
            // Configure the db context and user manager to use a single instance per request
           // app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>((options, context) =>
            {
                return (ApplicationUserManager)aumf.Create(options, context);
            });

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new ApplicationOAuthProvider("GameApi", (ISessionManagerClientFactory)unityResolver.GetService(typeof(ISessionManagerClientFactory))),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }


    }
}
