using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Xml.Serialization;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Unity.WebApi;
using System.Web.Http.Cors;
using Microsoft.Owin.Cors;
using System.Web.Cors;
using System.Threading.Tasks;
using System.Threading;
using EntityFX.Gdcame.Application.WebApi.Providers;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    public class WebApiStartup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }


        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {

            if (RuntimeHelper.IsRunningOnMono())
            {
                appBuilder.UseAesDataProtectorProvider();
            }

            /*appBuilder.Use((c, t) =>
            {
                return Task.FromResult(0);
            });*/

            // Configure Web API for self-host. 
            var container = new UnityContainer();
            var config = new HttpConfiguration();
            config.DependencyResolver = new UnityDependencyResolver(container);
            (new ContainerBootstrapper()).Configure(container);

            config.Filters.Add(new SessionExceptionHandlerFilterAttribute());
            config.Filters.Add(container.Resolve<GlobalExceptionHandlerFilterAttribute>());




            var corsPolicy = new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE");

            // Cors for the Asp.Net Identity (OAuth handler)
            appBuilder.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request =>
                        request.Path.Value == "/token" ?
                        corsPolicy.GetCorsPolicyAsync(null, CancellationToken.None) :
                        Task.FromResult<CorsPolicy>(null)
                }
            });

            // Cors for the WebApi
            config.EnableCors(corsPolicy);



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
            appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions)
                .UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );


            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;

            appBuilder.UseWebApi(config);
        }
    }
}