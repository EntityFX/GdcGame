using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
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
            var signalRPort = int.Parse(ConfigurationManager.AppSettings["SignalRPort"]);

            var webApiStartOptions = new StartOptions
            {
                Port = int.Parse(ConfigurationManager.AppSettings["WebApiPort"]),
            };

            webApiStartOptions.Urls.Add(string.Format("http://+:{0}", webApiStartOptions.Port));
            if (RuntimeHelper.IsRunningOnMono())
            {
                webApiStartOptions.ServerFactory = "Nowin";
            }

            // Start OWIN host 
            var httpWebApi = WebApp.Start<WebApiStartup>(webApiStartOptions);

            Console.WriteLine(RuntimeHelper.GetRuntimeInfo());
            Console.WriteLine("SignalR server running on {0}", signalRPort);
            Console.WriteLine("Web server running on {0}", webApiStartOptions.Port);
            Console.WriteLine("Repository provider: {0}", ConfigurationManager.AppSettings["RepositoryProvider"]);
            Console.ReadLine();
        }
    }
}