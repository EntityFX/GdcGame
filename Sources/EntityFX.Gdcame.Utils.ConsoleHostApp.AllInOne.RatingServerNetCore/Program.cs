using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Utils.Common;
using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.RatingServerNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var runtimeHelper = new RuntimeHelper();

            var appConfiguration = new AppConfiguration();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            appConfiguration.RepositoryProvider = configuration["RepositoryProvider"];
            appConfiguration.WebApiPort = Convert.ToInt32(configuration["WebApiPort"]);
            appConfiguration.WebServer = configuration["WebServer"];
            appConfiguration.MongoConnectionString = configuration["MongoConnectionString"];

            var host = new HostService(runtimeHelper, appConfiguration, new AutofacIocContainer(new ContainerBuilder()));
            host.Start();
        }
    }
}
