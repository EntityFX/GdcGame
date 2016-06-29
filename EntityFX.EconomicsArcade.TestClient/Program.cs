using System;
using System.Timers;

using System.Configuration;

using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
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


            var gr = new GameRunner(sessionGuid);
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

    class GameRunner
    {
        private readonly IGameManager _game;

        public GameRunner(Guid sessionGuid)
        {
            _game = new GameManagerClient(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"]
                , sessionGuid
                );
        }

        public void PerformManualStep()
        {
            _game.PerformManualStep();
            DisplayGameData(GetGameData());
        }

        public void BuyFundDriver(ConsoleKeyInfo keyInfo)
        {
            _game.BuyFundDriver((int)keyInfo.Key - 64);
            DisplayGameData(GetGameData());
        }

        public void FightAgainstCorruption()
        {
            _game.FightAgainstInflation();
            DisplayGameData(GetGameData());
        }

        public GameData GetGameData()
        {
            return _game.GetGameData();
        }

        public void PerformFiveYearPlan()
        {
            _game.ActivateDelayedCounter(3);
            DisplayGameData(GetGameData());
        }

        private bool IsFundsDriverAvailableForBuy(CounterBase rootCounter, FundsDriver fundsDriver)
        {
            return fundsDriver.UnlockValue <= rootCounter.Value;
        }

        private bool IsCounterWithInflation(GenericCounter counter)
        {
            return counter.Inflation > 0;
        }

        private bool IsCounterIsMining(DelayedCounter counter)
        {
            return counter.SecondsRemaining > 0;
        }

        private string GetIncrementorValueById(FundsDriver fundsDriver, int incrmentorId)
        {
            var incrementor = fundsDriver.Incrementors.ContainsKey(incrmentorId) ?
                fundsDriver.Incrementors[incrmentorId] : null;
            if (incrementor != null)
            {
                return String.Format("{0}{1}", incrementor.Value, incrementor.IncrementorType == IncrementorTypeEnum.PercentageIncrementor ? "%" : string.Empty);
            }
            return "0";
        }

        public void DisplayGameData(GameData gameData)
        {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Funds: {0:C}; Total Funds: {1:C}", gameData.Counters.CurrentFunds, gameData.Counters.TotalFunds);
                Console.WriteLine("Manual Steps: {0}, Automatic Steps: {1}",
                    gameData.ManualStepsCount, gameData.AutomaticStepsCount);
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "{1,15}: {0,12}", gameData.Counters.Counters[0].Value, gameData.Counters.Counters[0].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ", ((GenericCounter)gameData.Counters.Counters[1]).SubValue, gameData.Counters.Counters[1].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter)gameData.Counters.Counters[1]).Bonus, "Bonus"
                    , ((GenericCounter)gameData.Counters.Counters[1]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{1,15}: {0,12}%"
                    , ((GenericCounter)gameData.Counters.Counters[1]).Inflation, "Inflation");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}", 0, ((GenericCounter)gameData.Counters.Counters[1]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                    , ((GenericCounter)gameData.Counters.Counters[1]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}", ((GenericCounter)gameData.Counters.Counters[2]).SubValue, gameData.Counters.Counters[2].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter)gameData.Counters.Counters[2]).Bonus, "Bonus"
                    , ((GenericCounter)gameData.Counters.Counters[2]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                    , ((GenericCounter)gameData.Counters.Counters[2]).Inflation, "Corruption");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}", 0, ((GenericCounter)gameData.Counters.Counters[2]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                    , ((GenericCounter)gameData.Counters.Counters[2]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: +{0,12:C} {2}", gameData.Counters.Counters[3].Value, gameData.Counters.Counters[3].Name, TimeSpan.FromSeconds(((DelayedCounter)gameData.Counters.Counters[3]).SecondsRemaining));

                Console.WriteLine();
                int charIndex = 65;
                PrettyConsole.WriteLineColor(IsCounterWithInflation((GenericCounter)gameData.Counters.Counters[2]) ? ConsoleColor.Yellow : ConsoleColor.DarkYellow, "{0,2}:             Fight Against Corruption", "*");
                PrettyConsole.WriteLineColor(IsCounterIsMining((DelayedCounter)gameData.Counters.Counters[3]) ? ConsoleColor.DarkMagenta : ConsoleColor.Magenta, "{0,2}:                    Do Five Year Plan", "+");
                foreach (var fundsDriver in gameData.FundsDrivers)
                {
                    if (!IsFundsDriverAvailableForBuy(gameData.Counters.Counters[0], fundsDriver))
                    {
                        PrettyConsole.WriteColor(ConsoleColor.DarkGray, "{0,2}:             Need money to buy:     {1,8}. x{2,-4} ", ((char)charIndex).ToString(), fundsDriver.UnlockValue, fundsDriver.BuyCount);
                    }
                    else
                    {
                        PrettyConsole.WriteColor(ConsoleColor.White, "{3,2}: {0,28} {1,15:C} x{2,-4} ", fundsDriver.Name, fundsDriver.Value, fundsDriver.BuyCount, ((char)charIndex).ToString());
                    }
                    PrettyConsole.WriteColor(ConsoleColor.Red, "+{0, -4} ", GetIncrementorValueById(fundsDriver, 0));
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "+{0, -7} ",  GetIncrementorValueById(fundsDriver, 1));
                    PrettyConsole.WriteColor(ConsoleColor.Green, "+{0,-7} ",  GetIncrementorValueById(fundsDriver, 2));
                    Console.WriteLine();
                    charIndex++;
                }
            }

    }

    internal static class PrettyConsole
    {
        private static readonly object LockObject = new { };

        public static void WriteColor(ConsoleColor color, string text)
        {
            WriteColor(color, text, null);
        }

        public static void WriteColor(ConsoleColor color, string text, params object[] args)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text, args);
            Console.ForegroundColor = fg;
        }

        public static void WriteLineColor(ConsoleColor color, string text)
        {
            WriteLineColor(color, text, null);
        }

        public static void WriteLineColor(ConsoleColor color, string text, params object[] args)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, args);
            Console.ForegroundColor = fg;
        }
    }
}
