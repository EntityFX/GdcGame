using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using EntityFX.Gdcame.Application.WebApi.Providers;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Unity.WebApi;
using System.Linq;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOneCore
{

    internal class Program
    {
        private static void Main()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("Config.json").Build();

            var signalRPort = int.Parse(configBuilder["AppConfiguration:SignalRPort"]);

            var webApiStartOptions = new StartOptions
            {
                Port = int.Parse(configBuilder["AppConfiguration:WebApiPort"]),
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

            CoreStartup.AppConfiguration = new AppConfiguration()
            {
                MongoConnectionString = configBuilder["AppConfiguration:MongoConnectionString"],
                RepositoryProvider = configBuilder["AppConfiguration:RepositoryProvider"]
            };

            Console.WriteLine(RuntimeHelper.GetRuntimeInfo());
            Console.WriteLine("SignalR server running on {0}", signalRPort);
            Console.WriteLine("Web server running on {0}", webApiStartOptions.Port);
            Console.WriteLine("Repository provider: {0}", configBuilder["AppConfiguration:RepositoryProvider"]);

            var host = new WebHostBuilder()
                .UseConfiguration(configBuilder)
                .UseKestrel()
                .UseStartup<CoreStartup>()
                .UseUrls(webApiStartOptions.Urls.ToArray())
                .Build();

            host.Run();
                

            // Start OWIN host 
            //var httpWebApi = WebApp.Start<WebApiStartup>(webApiStartOptions);


        }
    }
}