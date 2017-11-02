using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using EntityFX.Gdcame.Application.Api.Common.Providers;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.MainServer;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Unity.WebApi;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    public class WebApiStartup
    {
        private readonly IIocContainer _unityContainer;
        private readonly AppConfiguration _appConfiguration;
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public WebApiStartup(IIocContainer unityContainer, AppConfiguration appConfiguration)
        {
            _unityContainer = unityContainer;
            _appConfiguration = appConfiguration;
        }

        class GlobalExceptionMiddleware : OwinMiddleware
        {
            public GlobalExceptionMiddleware(OwinMiddleware next) : base(next)
            { }

            public override async Task Invoke(IOwinContext context)
            {
                try
                {
                    await Next.Invoke(context);
                }
                catch (Exception ex)
                {
                    // your handling logic
                }
            }
        }


        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            if (RuntimeHelper.IsRunningOnMono())
            {
                appBuilder.UseAesDataProtectorProvider();
            }
            var config = new HttpConfiguration();
            config.DependencyResolver = new Common.UnityDependencyResolver((_unityContainer as IIocContainer<IUnityContainer>).Container);
            //config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver((_unityContainer as IIocContainer<IUnityContainer>).Container);
            config.Filters.Add(_unityContainer.Resolve<SessionExceptionHandlerFilterAttribute>());
            config.Filters.Add(new ValidateModelAttribute());



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
                    config.DependencyResolver.GetService(typeof(ApplicationUserManagerFacotory));

            appBuilder.CreatePerOwinContext<ApplicationUserManager>(
                (options, context) => (ApplicationUserManager)aumf.Create(options, context));

            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),

                Provider =
                    new CustomOAuthProvider("GameApi",
                        (ISessionManagerClientFactory)
                            config.DependencyResolver.GetService(typeof(ISessionManagerClientFactory)))
            };


            // Token Generation
            appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions)
                .UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
                );


            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;

            appBuilder.UseWebApi(config);
        }
    }
}