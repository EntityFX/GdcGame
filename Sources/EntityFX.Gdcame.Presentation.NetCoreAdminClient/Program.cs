using System;
using System.IO;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Presentation.Shared.AdminConsole;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;
using Microsoft.Extensions.Configuration;
using NLog;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Presentation.NetCoreAdminClient
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Enter login: ");
            var userName = Console.ReadLine();

            Console.Write("Enter password: ");
            var userPassword = Console.ReadLine();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var settings = new Settings()
            {
                GameServicePort = Convert.ToInt32(configuration["gameServerPort"]),
                GameServer = configuration["gameServer"],
                RatingServerPort = Convert.ToInt32(configuration["ratingServerPort"]),
                RatingServer = configuration["ratingServer"]
            };

            var logger = new NLoggerAdapter(LogManager.GetLogger("logger"));
            var ac = new AdminConsole(new ApiHelper<IAuthenticator>(logger, new RestsharpOAuth2ProviderFactory(logger), new RestsharpApiClientFactory()),
                userName, userPassword, settings);
            ac.StartMenu();
        }
    }
}
