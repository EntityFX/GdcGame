using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using EntityFX.Gdcame.Application.WebApi.Providers;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Owin.Security.AesDataProtectorProvider;

using Unity.WebApi;


namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using DataProtectionProviderDelegate = Func<string[], Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>>;
    using DataProtectionTuple = Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>;

    public static class KatanaIApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAppBuilder(this IApplicationBuilder app, Action<IAppBuilder> configure)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var appBuilder = new AppBuilder();
                    appBuilder.Properties["builder.DefaultApp"] = next;

                    configure(appBuilder);

                    return appBuilder.Build<AppFunc>();
                });
            });
            return app;
        }

        public static IAppBuilder SetDataProtectionProvider(this IAppBuilder appBuilder, IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.DataProtection.IDataProtectionProvider>();
            appBuilder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
            {
                var dataProtection = provider.CreateProtector(string.Join(",", purposes));
                return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
            });
            return appBuilder;
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAppBuilder(appBuilder =>
            {
                /*if (RuntimeHelper.IsRunningOnMono())
                {
                    appBuilder.UseAesDataProtectorProvider();
                }
                else
                {
                    
                }*/
                appBuilder.SetDataProtectionProvider(app);

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
                        config.DependencyResolver.GetService(typeof(ApplicationUserManagerFacotory));
                // Configure the db context and user manager to use a single instance per request
                // app.CreatePerOwinContext(ApplicationDbContext.Create);
                appBuilder.CreatePerOwinContext<ApplicationUserManager>(
                    (options, context) => { return (ApplicationUserManager)aumf.Create(options, context); });

                var OAuthServerOptions = new OAuthAuthorizationServerOptions
                {
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
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
            });
        }
    }

    internal class Program
    {
        private static void Main()
        {
            var signalRPort = int.Parse(ConfigurationManager.AppSettings["SignalRPort"]);

            var webApiStartOptions = new StartOptions
            {
                Port = int.Parse(ConfigurationManager.AppSettings["WebApiPort"]),
            };

            if (RuntimeHelper.IsRunningOnMono())
            {
                //webApiStartOptions.ServerFactory = "Nowin";
                webApiStartOptions.Urls.Add(string.Format("http://+:{0}", webApiStartOptions.Port));
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    webApiStartOptions.Urls.Add(string.Format("http://127.0.0.1:{0}", webApiStartOptions.Port));
                }
            } else
            {
                webApiStartOptions.Urls.Add(string.Format("http://+:{0}", webApiStartOptions.Port));
            }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls(webApiStartOptions.Urls.ToArray())
                .Build();

            host.Run();
                

            // Start OWIN host 
            //var httpWebApi = WebApp.Start<WebApiStartup>(webApiStartOptions);

            Console.WriteLine(RuntimeHelper.GetRuntimeInfo());
            Console.WriteLine("SignalR server running on {0}", signalRPort);
            Console.WriteLine("Web server running on {0}", webApiStartOptions.Port);
            Console.WriteLine("Repository provider: {0}", ConfigurationManager.AppSettings["RepositoryProvider"]);
            Console.ReadLine();
        }
    }
}