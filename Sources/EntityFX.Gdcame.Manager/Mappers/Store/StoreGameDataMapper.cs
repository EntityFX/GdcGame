using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using CounterBase = EntityFX.Gdcame.Common.Contract.Counters.CounterBase;
using FundsCounters = EntityFX.Gdcame.Common.Contract.Counters.FundsCounters;

namespace EntityFX.Gdcame.GameEngine.Mappers
{
    public class StoreGameDataMapper : IMapper<IGame, StoreGameData>
    {
        public StoreGameData Map(IGame source, StoreGameData destination = null)
        {
            var gameData = new StoreGameData()
            {
                Counters = new StoreFundsCounters()
                {
                    CurrentFunds = source.FundsCounters.CurrentFunds,
                    TotalFunds = source.FundsCounters.TotalFunds,
                    Counters = PrepareCountersToPersist(source)
                },
                AutomaticStepsCount = source.AutomaticStepNumber,
                ManualStepsCount = source.ManualStepNumber,
            };
            return gameData;
        }

        private StoreCounterBase[] PrepareCountersToPersist(IGame game)
        {
            var counters = new StoreCounterBase[game.FundsCounters.Counters.Count];
            foreach (var sourceCounter in game.FundsCounters.Counters)
            {
                StoreCounterBase destinationCouner = null;
                var sourcenGenericCounter = sourceCounter.Value as GenericCounter;
                if (sourcenGenericCounter != null)
                {
                    var destinationGenericCounter = new EntityFX.Gdcame.DataAccess.Contract.GameData.StoreGenericCounter
                    {
                        BonusPercent = sourcenGenericCounter.BonusPercentage,
                        Inflation = sourcenGenericCounter.Inflation,
                        CurrentSteps = sourcenGenericCounter.CurrentSteps
                    };
                    destinationCouner = destinationGenericCounter;
                    destinationCouner.Type = 1;
                }
                var sourceSingleCounter = sourceCounter.Value as SingleCounter;
                if (sourceSingleCounter != null)
                {
                    destinationCouner = new EntityFX.Gdcame.DataAccess.Contract.GameData.StoreSingleCounter();
                    destinationCouner.Type = 0;
                }
                var sourceDelayedCounter = sourceCounter.Value as DelayedCounter;
                if (sourceDelayedCounter != null)
                {
                    var destinationDelayedCounter = new EntityFX.Gdcame.DataAccess.Contract.GameData.StoreDelayedCounter()
                    {
                        SecondsRemaining = sourceDelayedCounter.SecondsRemaining,
                        DelayedValue = sourceDelayedCounter.SubValue
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