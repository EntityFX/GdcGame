using System;
using System.Web.Http;
using EntityFX.Gdcame.Presentation.Web.WebApp.EntityFX.EconomicsArcade.Presentation.GameApi;
using EntityFX.Gdcame.Presentation.Web.WebApp.Providers;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Unity.WebApi;

namespace EntityFX.Gdcame.Presentation.Web.WebApp
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
