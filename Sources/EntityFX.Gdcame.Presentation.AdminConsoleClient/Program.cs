using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            serverPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePort"]);
            mainServer = ConfigurationManager.AppSettings["Server"];
            ratingServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["RatingServerPort"]);
            ratingServer = ConfigurationManager.AppSettings["RatingServer"];

            Console.Write("Enter login: ");
            userName = Console.ReadLine();

            Console.Write("Enter password: ");
            userPassword = Console.ReadLine();

            var ac = new AdminConsole(
                userName, userPassword, 
                new Uri(string.Format("{0}:{1}", mainServer, serverPort)),
                new Uri(string.Format("{0}:{1}", ratingServer, ratingServerPort)),
                serverPort, ratingServerPort);
            ac.StartMenu();
        }
    }
}
