using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Utils.Common;
using Fos;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Unity.WebApi;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    internal class Program
    {
        private static void Main()
        {
            var baseHost = "+";


            var signalRPort = int.Parse(ConfigurationManager.AppSettings["SignalRPort"]);

            var webApiStartOptions = new StartOptions
            {
                Port = int.Parse(ConfigurationManager.AppSettings["WebApiPort"])
            };

            if (RuntimeHelper.IsRunningOnMono())
            {
                webApiStartOptions.ServerFactory = "Nowin";
            }

            // Start OWIN host 
            var httpWebApi = WebApp.Start<WebApiStartup>(webApiStartOptions);

            //using (var fosServer = new FosSelfHost(appBuilder =>
            //{
            //    if (RuntimeHelper.IsRunningOnMono())
            //    {
            //        appBuilder.UseAesDataProtectorProvider();
            //    }
            //    appBuilder.Properties["port"] = 9001;
            //    // Configure Web API for self-host. 
            //    var config = new HttpConfiguration();

            //    var container = new UnityContainer();

            //    config.DependencyResolver = new UnityDependencyResolver(container);

            //    (new ContainerBootstrapper()).Configure(container);

            //    var aumf =
            //        (ApplicationUserManagerFacotory)
            //            config.DependencyResolver.GetService(typeof(ApplicationUserManagerFacotory));
            //    // Configure the db context and user manager to use a single instance per request
            //    // app.CreatePerOwinContext(ApplicationDbContext.Create);
            //    appBuilder.CreatePerOwinContext<ApplicationUserManager>(
            //        (options, context) => { return (ApplicationUserManager)aumf.Create(options, context); });

            //    var OAuthServerOptions = new OAuthAuthorizationServerOptions
            //    {
            //        AllowInsecureHttp = true,
            //        TokenEndpointPath = new PathString("/token"),
            //        AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            //        Provider =
            //            new CustomOAuthProvider("GameApi",
            //                (ISessionManagerClientFactory)
            //                    config.DependencyResolver.GetService(typeof(ISessionManagerClientFactory)))
            //    };

            //    // Token Generation
            //    appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions);
            //    appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //    config.MapHttpAttributeRoutes();

            //    config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
            //        );


            //    var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //    jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    jsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            //    appBuilder.UseWebApi(config);
            //}))
            //{
            //    fosServer.Bind(System.Net.IPAddress.Loopback, 9000);
            //    fosServer.Start(false);
            //}

            //var signalR = WebApp.Start(string.Format("http://{0}:{1}", baseHost, signalRPort), builder =>
            //{
            //    var listener = (HttpListener)builder.Properties[typeof(HttpListener).FullName];
            //    listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            //    builder.UseCors(CorsOptions.AllowAll);
            //    builder.MapSignalR();
            //    builder.RunSignalR(new HubConfiguration
            //    {
            //        EnableDetailedErrors = true,
            //        EnableJSONP = true
            //    });
            //});
            Console.WriteLine(RuntimeHelper.GetRuntimeInfo());
            Console.WriteLine("SignalR server running on {0}", signalRPort);
            Console.WriteLine("Web server running on {0}", webApiStartOptions.Port);


            Console.ReadLine();
        }
    }
}