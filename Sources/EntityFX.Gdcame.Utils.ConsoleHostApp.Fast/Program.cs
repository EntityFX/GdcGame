﻿using System;
using System.Linq;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin.Hosting;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Fast.MainServer
{
    class Program
    {
        static void Main(string[] args)
        {

            var runtimeHelper = new RuntimeHelper();

            var _webApiStartOptions = new StartOptions
            {
                Port = 80,
            };

            if (runtimeHelper.IsRunningOnMono())
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
                    options.Limits.MaxRequestBufferSize = 4*1014*1024;
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
