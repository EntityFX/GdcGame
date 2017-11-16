using System;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;
using EntityFX.Gdcame.Presentation.ClientShared;
namespace EntityFX.Gdcame.Presentation.ConsoleClient
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.SetWindowSize(100, 50);
            var listArgs = args.ToList();
            if (args.Length > 0)
                foreach (var arg in args)
                    listArgs.Remove(arg);

            var settings = new Settings()
            {
                GameServicePort = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePort"]),
                GameServer = ConfigurationManager.AppSettings["Server"],
                RatingServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["RatingServerPort"]),
                RatingServer = ConfigurationManager.AppSettings["RatingServer"]
            };

            (new EntityFX.Gdcame.Presentation.ClientShared.GameConsole(settings)).MainLoop(listArgs);
        }
    }


}