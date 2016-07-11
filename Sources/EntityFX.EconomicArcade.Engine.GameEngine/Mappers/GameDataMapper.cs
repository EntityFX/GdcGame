using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicArcade.Engine.GameEngine.Mappers
{
    public class GameDataMapper : IMapper<IGame, GameData>
    {
        public GameData Map(IGame source, GameData destination = null)
        {
            return new GameData()
            {
                Counters = new EconomicsArcade.Contract.Common.Counters.FundsCounters()
                {
                    CurrentFunds = source.FundsCounters.CurrentFunds,
                    TotalFunds = source.FundsCounters.TotalFunds,
                    Counters = PrepareCountersToPersist(source)
                },
                AutomaticStepsCount = source.AutomaticStepNumber,
                ManualStepsCount =  source.ManualStepNumber
            };
        }

        private EconomicsArcade.Contract.Common.Counters.CounterBase[] PrepareCountersToPersist(IGame game)
        {
            var counters = new EconomicsArcade.Contract.Common.Counters.CounterBase[game.FundsCounters.Counters.Count];
            foreach (var sourceCounter in game.FundsCounters.Counters)
            {
                EconomicsArcade.Contract.Common.Counters.CounterBase destinationCouner = null;
                var sourcenGenericCounter = sourceCounter.Value as GenericCounter;
                if (sourcenGenericCounter != null)
                {
                    var destinationGenericCounter = new EconomicsArcade.Contract.Common.Counters.GenericCounter
                    {
                        BonusPercentage = sourcenGenericCounter.BonusPercentage,
                        Inflation = sourcenGenericCounter.Inflation,
                        CurrentSteps = sourcenGenericCounter.CurrentSteps,
                        SubValue = sourcenGenericCounter.SubValue
                    };
                    destinationCouner = destinationGenericCounter;
                    destinationCouner.Type = 1;
                }
                var sourceSingleCounter = sourceCounter.Value as SingleCounter;
                if (sourceSingleCounter != null)
                {
                    destinationCouner = new EconomicsArcade.Contract.Common.Counters.SingleCounter();
                    destinationCouner.Type = 0;
                }
                var sourceDelayedCounter = sourceCounter.Value as DelayedCounter;
                if (sourceDelayedCounter != null)
                {
                    var destinationDelayedCounter = new EconomicsArcade.Contract.Common.Counters.DelayedCounter()
                    {
                        SecondsRemaining = sourceDelayedCounter.SecondsRemaining
                    };
                    destinationCouner = destinationDelayedCounter;
                    destinationCouner.Type = 2;
                }
                if (destinationCouner != null)
                {
                    destinationCouner.Id = sourceCounter.Value.Id;
                    destinationCouner.Value = sourceCounter.Value.Value;
                }
                if (destinationCouner != null) counters[destinationCouner.Id] = destinationCouner;
            }
            return counters;
        }
    }
}