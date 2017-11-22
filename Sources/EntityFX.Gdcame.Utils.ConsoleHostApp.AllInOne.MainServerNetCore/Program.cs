

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Infrastructure;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServerNetCore
{
    class Program
    {
        static void Main(string[] args)
        {

            //var host1 = new WebHostBuilder()
            //    .UseKestrel(
            //            options =>
            //            {
            //                options.UseConnectionLogging();
            //                options.ThreadCount = 32;
            //                options.Limits.MaxRequestBufferSize = 4 * 1014 * 1024;
            //            })
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseStartup(typeof(Startup).GetTypeInfo().Assembly.GetName().Name).UseUrls(new[] { "http://+:9999" })
            //    .UseEnvironment("Development")
            //    .Build();

            //host1.Run();
   


            var runtimeHelper = new RuntimeHelper();


            var appConfiguration = new AppConfiguration()
            {
                MongoConnectionString = "mongodb://gdcame-devnode:27017/gdcame",
                RepositoryProvider = "Mongo",
                WebApiPort = 9080,
                SignalRPort = 9081,
                WebServer = "Kestrel",
                KestrelThreads = 32
            };

            var host = new HostService(runtimeHelper, appConfiguration, new AutofacIocContainer(new ContainerBuilder()));
            host.Start();
        }
    }

    //    public class SomeLogger : ILogger
    //    {
    //        public void Debug(string message, params object[] args)
    //        {

    //        }

    //        public void Trace(string message, params object[] args)
    //        {

    //        }

    //        public void Info(string message, params object[] args)
    //        {

    //        }

    //        public void Warning(string message, params object[] args)
    //        {

    //        }

    //        public void Error(Exception exception)
    //        {

    //        }
    //    }

    //    public class CoreCache : ICache
    //    {
    //        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
    //        public void Set(string key, object value, DateTimeOffset period)
    //        {
    //            _cache[key] = value;
    //        }

    //        public object Get(string key)
    //        {
    //            return _cache[key];
    //        }

    //        public void Remove(string key)
    //        {
    //            _cache.Remove(key);
    //        }

    //        public bool Contains(string key)
    //        {
    //            return _cache.ContainsKey(key);
    //        }
    //    }

    //    public class SomeRuntimeHelper : IRuntimeHelper
    //    {
    //        public string GetRuntimeName()
    //        {
    //            return "0";
    //        }

    //        public string GetRuntimeInfo()
    //        {
    //            return "0";
    //        }

    //        public long GetTotalMemoryInMb()
    //        {
    //            return 0;
    //        }

    //        public float GetCpuUsage()
    //        {
    //            return 0;
    //        }

    //        public float GetAvailablememoryInMb()
    //        {
    //            return 0;
    //        }

    //        public float GetMemoryUsageInMb()
    //        {
    //            return 0;
    //        }

    //        public bool IsRunningOnMono()
    //        {
    //            return false;
    //        }
    //    }
    //}
}
