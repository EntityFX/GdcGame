﻿using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using IclServices.WcfTest.TestClient;
using PortableLog.NLog;

namespace EntityFX.EconomicsArcade.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MainLoop(args);

            //Console.ReadKey();
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
                new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))
                , ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"]
                 , sessionGuid
                 );

            var adminManagerClient = new AdminManagerClient(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_AdminManager"]
                );

            var gr = new GameRunner(userName, sessionGuid, gameClient);
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
                else if (keyInfo.Key == ConsoleKey.F2)
                {
                    var terminal = new UIConsole(adminManagerClient);
                    terminal.StartMenu();

                    gr.Invalidate();
                }
            }
        }
    }
}
