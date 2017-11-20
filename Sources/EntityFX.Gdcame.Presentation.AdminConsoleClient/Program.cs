using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;
using EntityFX.Presentation.Shared.AdminConsole;
using NLog;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Presentation.AdminConsoleClient
{
    class Program
    {
        private static int serverPort;
        private static string mainServer;
        private static int ratingServerPort;
        private static string ratingServer;
        private static string userName;
        private static string userPassword;

        static void Main(string[] args)
        {

            Console.Write("Enter login: ");
            userName = Console.ReadLine();

            Console.Write("Enter password: ");
            userPassword = Console.ReadLine();

            var settings = new Settings()
            {
                GameServer = ConfigurationManager.AppSettings["Server"],
                GameServicePort = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePort"]),
                RatingServer = ConfigurationManager.AppSettings["RatingServer"],
                RatingServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["RatingServerPort"]),
            };
            //ApiHelper<IAuthenticator> apiHelper, string user, string password, Uri mainServer, Uri ratingServer, int port, int ratingServerPort
            var logger = new NLoggerAdapter(LogManager.GetLogger("logger"));
            var ac = new AdminConsole(new ApiHelper<IAuthenticator>(logger, new RestsharpOAuth2ProviderFactory(logger), new RestsharpApiClientFactory() ), 
                userName, userPassword, settings);
            ac.StartMenu();
        }
    }
}
