using System;
using EntityFx.GdCame.Presentation.Shared;
using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Presentation.Shared.GameConsole
{

    public abstract class GameRunnerBase
    {
        protected static readonly object _stdLock = new { };

        private bool IsFundsDriverAvailableForBuy(CounterModelBase rootCounter, ItemModel item)
        {
            return item.UnlockBalance <= rootCounter.Value;
        }

        private bool IsCounterAvailableForActivate(DelayedCounterModel counter, decimal currentFunds)
        {
            return counter.UnlockValue <= currentFunds;
        }

        private bool IsCounterWithInflation(GenericCounterModel counter)
        {
            return counter.Inflation > 0;
        }

        private bool IsCounterIsMining(DelayedCounterModel counter)
        {
            return counter.SecondsRemaining > 0;
        }

        private string GetIncrementorValueById(ItemModel item, int incrmentorId)
        {
            var incrementor = item.Incrementors.ContainsKey(incrmentorId)
                ? item.Incrementors[incrmentorId]
                : null;
            if (incrementor != null)
            {
                return incrementor;
            }
            return "0";
        }

        public abstract GameDataModel GetGameData();

        public virtual void DisplayGameData(GameDataModel gameData)
        {
            lock (_stdLock)
            {
                Console.WriteLine("Balance: {0:C}; Total earned: {1:C}", gameData.Cash.OnHand,
                    gameData.Cash.TotalEarned);
                /*Console.WriteLine("Manual Steps: {0}, Automatic Steps: {1}",
                    gameData.ManualStepsCount, gameData.AutomatedStepsCount);*/
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "{1,15}: {0,12}", gameData.Cash.Counters[0].Value,
                    gameData.Cash.Counters[0].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ",
                    ((GenericCounterModel)gameData.Cash.Counters[1]).SubValue, gameData.Cash.Counters[1].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounterModel)gameData.Cash.Counters[1]).Bonus, "Bonus"
                    , ((GenericCounterModel)gameData.Cash.Counters[1]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{1,15}: {0,12}%"
                    , ((GenericCounterModel)gameData.Cash.Counters[1]).Inflation, "Buthurt");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                    , ((GenericCounterModel)gameData.Cash.Counters[1]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}",
                    ((GenericCounterModel)gameData.Cash.Counters[2]).SubValue, gameData.Cash.Counters[2].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounterModel)gameData.Cash.Counters[2]).Bonus, "Bonus"
                    , ((GenericCounterModel)gameData.Cash.Counters[2]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                    , ((GenericCounterModel)gameData.Cash.Counters[2]).Inflation, "Fatigue");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                    , ((GenericCounterModel)gameData.Cash.Counters[2]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: +{0,12:C} {2}/{3}",
                    gameData.Cash.Counters[3].Value, gameData.Cash.Counters[3].Name
                    , TimeSpan.FromSeconds(((DelayedCounterModel)gameData.Cash.Counters[3]).SecondsRemaining)
                    , 0);

                Console.WriteLine();
                var charIndex = 65;
                PrettyConsole.WriteLineColor(
                    IsCounterWithInflation((GenericCounterModel)gameData.Cash.Counters[2])
                        ? ConsoleColor.Yellow
                        : ConsoleColor.DarkYellow, "{0,2}:   Do rest", "*");
                if (IsCounterAvailableForActivate((DelayedCounterModel)gameData.Cash.Counters[3],
                    gameData.Cash.Counters[0].Value))
                {
                    PrettyConsole.WriteLineColor(
                        IsCounterIsMining((DelayedCounterModel)gameData.Cash.Counters[3])
                            ? ConsoleColor.DarkMagenta
                            : ConsoleColor.Magenta, "{0,2}:         Do quarter goal", "+");
                }
                else
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkMagenta,
                        "{0,2}:         To do quarter goal need {1} {2}", "+",
                        ((DelayedCounterModel)gameData.Cash.Counters[3]).UnlockValue, gameData.Cash.Counters[0].Name);
                }
                foreach (var fundsDriver in gameData.Items)
                {
                    if (!IsFundsDriverAvailableForBuy(gameData.Cash.Counters[0], fundsDriver))
                    {
                        PrettyConsole.WriteColor(ConsoleColor.Gray,
                            "{3,2}: {0,26} {1,-30}", fundsDriver.Name, "Need to buy: " + fundsDriver.UnlockBalance,
                             fundsDriver.Bought, ((char)charIndex).ToString());
                    }
                    else
                    {
                        PrettyConsole.WriteColor(ConsoleColor.White, "{3,2}: {0,26} {1,24} x{2,-4} ", fundsDriver.Name,
                            FormatMoney(fundsDriver.Price), fundsDriver.Bought, ((char)charIndex).ToString());
                    }
                    PrettyConsole.WriteColor(ConsoleColor.Red, "+{0, -4} ", GetIncrementorValueById(fundsDriver, 0));
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "+{0, -7} ", GetIncrementorValueById(fundsDriver, 1));
                    PrettyConsole.WriteColor(ConsoleColor.Green, "+{0,-7} ", GetIncrementorValueById(fundsDriver, 2));
                    Console.WriteLine();
                    charIndex++;
                }
            }
        }

        private static string FormatMoney(decimal money)
        {
            if (money < 1000)
            {
                return string.Format("{0:N1}", money);
            }
            var tCount = 1;
            while ((money = money / 1000) > 1000 && tCount < 5)
            {
                tCount++;
            }

            string suffix = string.Empty;
            switch (tCount)
            {
                case 1:
                    suffix = "k";
                    break;
                case 2:
                    suffix = "kk";
                    break;
                case 3:
                    suffix = "kkk";
                    break;
                case 4:
                    suffix = "kkkk";
                    break;
                default:
                    break;
            }
            return string.Format("{0:N1}{1}", money, suffix);
        }
    }
}