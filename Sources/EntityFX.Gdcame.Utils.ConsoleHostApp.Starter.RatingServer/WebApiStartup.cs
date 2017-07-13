using System.Linq;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using EntityFX.Gdcame.Application.Api.Common.Providers;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Owin.Cors;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Unity.WebApi;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer
{
    public class WebApiStartup
    {
        private readonly IUnityContainer _unityContainer;
        private readonly AppConfiguration _appConfiguration;

        public WebApiStartup(IUnityContainer unityContainer, AppConfiguration appConfiguration)
        {
            _unityContainer = unityContainer;
            _appConfiguration = appConfiguration;
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
            config.DependencyResolver = new UnityDependencyResolver(_unityContainer);
            config.Filters.Add(new ValidateModelAttribute());


            var corsPolicy = new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE");

            // Cors for the WebApi
            config.EnableCors(corsPolicy);





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