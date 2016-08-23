using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using Owin;
using Unity.WebApi;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }


        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            var container = new UnityContainer();

            config.DependencyResolver = new UnityDependencyResolver(container);

            (new ContainerBootstrapper()).Configure(container);

            var aumf =
                (ApplicationUserManagerFacotory)
                    config.DependencyResolver.GetService(typeof (ApplicationUserManagerFacotory));
            // Configure the db context and user manager to use a single instance per request
            // app.CreatePerOwinContext(ApplicationDbContext.Create);
            appBuilder.CreatePerOwinContext<ApplicationUserManager>(
                (options, context) => { return (ApplicationUserManager) aumf.Create(options, context); });

            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider =
                    new CustomOAuthProvider("GameApi",
                        (ISessionManagerClientFactory)
                            config.DependencyResolver.GetService(typeof (ISessionManagerClientFactory)))
            };

            // Token Generation
            appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions);
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );


            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            appBuilder.UseWebApi(config);
        }
    }
}