using System.Collections.Generic;
using System.Net;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using EntityFX.Gdcame.Application.WebApi.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Topshelf;
using Topshelf.Logging;
using Unity.WebApi;


namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(configureCallback =>
            {
                configureCallback.RunAsNetworkService();
                configureCallback.StartAutomatically();
                configureCallback.UseNLog();
                configureCallback.SetServiceName("GDCame");
                configureCallback.Service<HostService>(service =>
                {
                    service.ConstructUsing(_ => new HostService());
                    service.WhenStarted((hostService, control) => hostService.Start(control));
                    service.WhenStopped((hostService, control) => hostService.Stop(control));
                });
                configureCallback.EnableServiceRecovery(configurator =>
                {
                    configurator.RestartService(1);
                    configurator.RestartComputer(10, "Holly shit: gdcame not respond too much - restart computer");
                });
            });

        }
    }
}