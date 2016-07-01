using System;
using System.Timers;

using System.Configuration;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;

namespace EntityFX.EconomicsArcade.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MainLoop(args);
        }

        static void MainLoop(string[] args)
        {
            string userName;
            if (args.Length < 1)
            {
                Console.Write("Please, enter user name: ");
                userName = Console.ReadLine();
            }
            else
            {
                userName = args[0];
            }

            var simpleUserManagerClient = new SimpleUserManagerClient(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"]
                );
            if (!simpleUserManagerClient.Exists(userName))
            {
                simpleUserManagerClient.Create(userName);
            }

            var sessionManagerClient = new SessionManagerClient(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"]
                );
            var sessionGuid = sessionManagerClient.AddSession(userName);

            var gameClient = new GameManagerClient(
                 ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"]
                 , sessionGuid
                 );

            var gr = new GameRunner(sessionGuid, gameClient);
            var gameData = gr.GetGameData();
            gr.DisplayGameData(gameData);
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            //while ((keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Enter, false, false, false)).Key != ConsoleKey.Escape)
            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    gr.PerformManualStep();
                }
                else if ((int)keyInfo.Key >= 65 && (int)keyInfo.Key <= 90)
                {
                    gr.BuyFundDriver(keyInfo);
                }
                else if (keyInfo.Key == ConsoleKey.Multiply)
                {
                    gr.FightAgainstCorruption();
                }
                else if (keyInfo.Key == ConsoleKey.Add)
                {
                    gr.PerformFiveYearPlan();
                }
                else if (keyInfo.Key == ConsoleKey.F5)
                {
                    gr.DisplayGameData(gr.GetGameData());
                }
            }
        }
    }
}
