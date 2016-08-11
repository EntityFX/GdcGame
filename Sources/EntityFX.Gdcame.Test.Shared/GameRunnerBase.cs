using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.Common.Contract.Incrementors;


namespace EntityFx.EconomicsArcade.Test.Shared
{
    public abstract class GameRunnerBase
    {
        public GameRunnerBase()
        {
        }

        private bool IsFundsDriverAvailableForBuy(CounterBase rootCounter, FundsDriver fundsDriver)
        {
            return fundsDriver.UnlockValue <= rootCounter.Value;
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

        public abstract GameData GetGameData();

        protected static readonly object _stdLock = new { };

        public virtual void DisplayGameData(GameData gameData)
        {
            lock (_stdLock)
            {
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
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}", ((GenericCounter)gameData.Counters.Counters[1]).InflationIncreaseSteps, ((GenericCounter)gameData.Counters.Counters[1]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                    , ((GenericCounter)gameData.Counters.Counters[1]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}", ((GenericCounter)gameData.Counters.Counters[2]).SubValue, gameData.Counters.Counters[2].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter)gameData.Counters.Counters[2]).Bonus, "Bonus"
                    , ((GenericCounter)gameData.Counters.Counters[2]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                    , ((GenericCounter)gameData.Counters.Counters[2]).Inflation, "Corruption");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}", ((GenericCounter)gameData.Counters.Counters[2]).InflationIncreaseSteps, ((GenericCounter)gameData.Counters.Counters[2]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                    , ((GenericCounter)gameData.Counters.Counters[2]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: +{0,12:C} {2}/{3}", gameData.Counters.Counters[3].Value, gameData.Counters.Counters[3].Name
                    , TimeSpan.FromSeconds(((DelayedCounter)gameData.Counters.Counters[3]).SecondsRemaining)
                    , TimeSpan.FromSeconds(((DelayedCounter)gameData.Counters.Counters[3]).MiningTimeSeconds));

                Console.WriteLine();
                int charIndex = 65;
                PrettyConsole.WriteLineColor(IsCounterWithInflation((GenericCounter)gameData.Counters.Counters[2]) ? ConsoleColor.Yellow : ConsoleColor.DarkYellow, "{0,2}:   Fight Against Corruption", "*");
                if (IsCounterAvailableForActivate((DelayedCounter)gameData.Counters.Counters[3], gameData.Counters.Counters[0].Value))
                {
                    PrettyConsole.WriteLineColor(IsCounterIsMining((DelayedCounter)gameData.Counters.Counters[3]) ? ConsoleColor.DarkMagenta : ConsoleColor.Magenta, "{0,2}:         Do Five Year Plan", "+");
                }
                else
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkMagenta, "{0,2}:         Do Five Year Plan: need {1} {2} to unlock", "+", ((DelayedCounter)gameData.Counters.Counters[3]).UnlockValue, gameData.Counters.Counters[0].Name);

                }
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
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "+{0, -7} ", GetIncrementorValueById(fundsDriver, 1));
                    PrettyConsole.WriteColor(ConsoleColor.Green, "+{0,-7} ", GetIncrementorValueById(fundsDriver, 2));
                    Console.WriteLine();
                    charIndex++;
                }
            }
        }
    }
}
