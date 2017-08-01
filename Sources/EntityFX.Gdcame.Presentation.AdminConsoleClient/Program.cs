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
        private static int _serverPort;
        private static string _mainServer;
        private static int _ratingServerPort;
        private static string _ratingServer;
        private static string _userName;
        private static string _userPassword;

        static void Main(string[] args)
        {
            _serverPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePort"]);
            _mainServer = ConfigurationManager.AppSettings["Server"];
            _ratingServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["RatingServerPort"]);
            _ratingServer = ConfigurationManager.AppSettings["RatingServer"];

            Console.Write("Enter login: ");
            _userName = Console.ReadLine();

            Console.Write("Enter password: ");
            _userPassword = Console.ReadLine();

            var ac = new AdminConsole(
                _userName, _userPassword, 
                new Uri(string.Format("{0}:{1}", _mainServer, _serverPort)),
                new Uri(string.Format("{0}:{1}", _ratingServer, _ratingServerPort)),
                _serverPort, _ratingServerPort);
            ac.StartMenu();
        }
    }
}
