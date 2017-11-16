using System;
using System.IO;
using EntityFX.Gdcame.Presentation.ClientShared;
using EntityFX.Gdcame.Presentation.ConsoleClient;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;
using Microsoft.Extensions.Configuration;

namespace EntityFX.Gdcame.Presentation.NetCoreConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
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

            (new GameConsole(settings)).MainLoop(args);
        }
    }
}
