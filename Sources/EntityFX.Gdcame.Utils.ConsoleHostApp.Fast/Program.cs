using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin.Hosting;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Fast
{
    class Program
    {
        static void Main(string[] args)
        {

            var _webApiStartOptions = new StartOptions
            {
                Port = 80,
            };

            if (RuntimeHelper.IsRunningOnMono())
            {
                //webApiStartOptions.ServerFactory = "Nowin";
                _webApiStartOptions.Urls.Add(string.Format("http://+:{0}", _webApiStartOptions.Port));
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    _webApiStartOptions.Urls.Add(string.Format("http://127.0.0.1:{0}", _webApiStartOptions.Port));
                }
            }
            else
            {
                _webApiStartOptions.Urls.Add(string.Format("http://+:{0}", _webApiStartOptions.Port));
            }

            using (var _webHost = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.MaxRequestBufferSize = 4*1014*1024;
                    options.UseConnectionLogging();
                    options.ThreadCount = Environment.ProcessorCount;
                })
                .UseStartup<CoreStartup>()
                .UseUrls(_webApiStartOptions.Urls.ToArray())
                .Build())
            {
                _webHost.Start();
                Console.ReadKey();
            }

        }
    }
}
