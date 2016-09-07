using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.Common.Contract.Items;

namespace EntityFx.GdCame.Test.Shared
{
    public abstract class GameRunnerBase
    {
        protected static readonly object _stdLock = new {};

        private bool IsFundsDriverAvailableForBuy(CounterBase rootCounter, Item item)
        {
            return item.UnlockBalance <= rootCounter.Value;
        }

        private bool IsCounterAvailableForActivate(DelayedCounter counter, decimal currentFunds)
        {
            return counter.UnlockValue <= currentFunds;
        }

        private bool IsCounterWithInflation(GenericCounter counter)
        {
            return counter.Inflation > 0;
        }

        private bool IsCounterIsMining(DelayedCounter counter)
        {
            return counter.SecondsRemaining > 0;
        }

        private string GetIncrementorValueById(Item item, int incrmentorId)
        {
            var incrementor = item.Incrementors.ContainsKey(incrmentorId)
                ? item.Incrementors[incrmentorId]
                : null;
            if (incrementor != null)
            {
                return string.Format("{0}{1}", incrementor.Value,
                    incrementor.IncrementorType == IncrementorTypeEnum.PercentageIncrementor ? "%" : string.Empty);
            }
            return "0";
        }

        public abstract GameData GetGameData();

        public virtual void DisplayGameData(GameData gameData)
        {
            lock (_stdLock)
            {
                Console.WriteLine("Funds: {0:C}; Total Funds: {1:C}", gameData.Cash.OnHand,
                    gameData.Cash.Total);
                Console.WriteLine("Manual Steps: {0}, Automatic Steps: {1}",
                    gameData.ManualStepsCount, gameData.AutomatedStepsCount);
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "{1,15}: {0,12}", gameData.Cash.Counters[0].Value,
                    gameData.Cash.Counters[0].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ",
                    ((GenericCounter) gameData.Cash.Counters[1]).SubValue, gameData.Cash.Counters[1].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter) gameData.Cash.Counters[1]).Bonus, "Bonus"
                    , ((GenericCounter) gameData.Cash.Counters[1]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{1,15}: {0,12}%"
                    , ((GenericCounter) gameData.Cash.Counters[1]).Inflation, "Inflation");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}",
                    ((GenericCounter) gameData.Cash.Counters[1]).InflationIncreaseSteps,
                    ((GenericCounter) gameData.Cash.Counters[1]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                    , ((GenericCounter) gameData.Cash.Counters[1]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}",
                    ((GenericCounter) gameData.Cash.Counters[2]).SubValue, gameData.Cash.Counters[2].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter) gameData.Cash.Counters[2]).Bonus, "Bonus"
                    , ((GenericCounter) gameData.Cash.Counters[2]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                    , ((GenericCounter) gameData.Cash.Counters[2]).Inflation, "Corruption");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}",
                    ((GenericCounter) gameData.Cash.Counters[2]).InflationIncreaseSteps,
                    ((GenericCounter) gameData.Cash.Counters[2]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                    , ((GenericCounter) gameData.Cash.Counters[2]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: +{0,12:C} {2}/{3}",
                    gameData.Cash.Counters[3].Value, gameData.Cash.Counters[3].Name
                    , TimeSpan.FromSeconds(((DelayedCounter) gameData.Cash.Counters[3]).SecondsRemaining)
                    , TimeSpan.FromSeconds(((DelayedCounter) gameData.Cash.Counters[3]).MiningTimeSeconds));

                Console.WriteLine();
                var charIndex = 65;
                PrettyConsole.WriteLineColor(
                    IsCounterWithInflation((GenericCounter) gameData.Cash.Counters[2])
                        ? ConsoleColor.Yellow
                        : ConsoleColor.DarkYellow, "{0,2}:   Fight Against Corruption", "*");
                if (IsCounterAvailableForActivate((DelayedCounter) gameData.Cash.Counters[3],
                    gameData.Cash.Counters[0].Value))
                {
                    PrettyConsole.WriteLineColor(
                        IsCounterIsMining((DelayedCounter) gameData.Cash.Counters[3])
                            ? ConsoleColor.DarkMagenta
                            : ConsoleColor.Magenta, "{0,2}:         Do Five Year Plan", "+");
                }
                else
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkMagenta,
                        "{0,2}:         Do Five Year Plan: need {1} {2} to unlock", "+",
                        ((DelayedCounter) gameData.Cash.Counters[3]).UnlockValue, gameData.Cash.Counters[0].Name);
                }
                foreach (var item in gameData.Items)
                {
                    if (!IsFundsDriverAvailableForBuy(gameData.Cash.Counters[0], item))
                    {
                        PrettyConsole.WriteColor(ConsoleColor.Gray,
                            "{0,2}:             Need money to buy:     {1,8}. x{2,-4} ", ((char) charIndex).ToString(),
                            item.UnlockBalance, item.Bought);
                    }
                    else
                    {
                        PrettyConsole.WriteColor(ConsoleColor.White, "{3,2}: {0,28} {1,15:C} x{2,-4} ", item.Name,
                            item.Price, item.Bought, ((char) charIndex).ToString());
                    }
                    PrettyConsole.WriteColor(ConsoleColor.Red, "+{0, -4} ", GetIncrementorValueById(item, 0));
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "+{0, -7} ", GetIncrementorValueById(item, 1));
                    PrettyConsole.WriteColor(ConsoleColor.Green, "+{0,-7} ", GetIncrementorValueById(item, 2));
                    Console.WriteLine();
                    charIndex++;
                }
            }
        }
    }
}