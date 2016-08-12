using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using CounterBase = EntityFX.Gdcame.Common.Contract.Counters.CounterBase;

namespace EntityFX.Gdcame.GameEngine.Mappers
{
    public class StoreGameDataMapper : IMapper<IGame, StoredGameData>
    {
        public StoredGameData Map(IGame source, StoredGameData destination = null)
        {
            var gameData = new StoredGameData()
            {
                Cash = new StoredCash()
                {
                    CashOnHand = source.GameCash.CashOnHand,
                    TotalEarned = source.GameCash.TotalEarned,
                    Counters = PrepareCountersToPersist(source)
                },
                AutomatedStepsCount = source.AutomaticStepNumber,
                ManualStepsCount = source.ManualStepNumber,
            };
            return gameData;
        }

        private StoredCounterBase[] PrepareCountersToPersist(IGame game)
        {
            var counters = new StoredCounterBase[game.GameCash.Counters.Count];
            foreach (var sourceCounter in game.GameCash.Counters)
            {
                StoredCounterBase destinationCouner = null;
                var sourcenGenericCounter = sourceCounter.Value as GenericCounter;
                if (sourcenGenericCounter != null)
                {
                    var destinationGenericCounter = new EntityFX.Gdcame.DataAccess.Contract.GameData.StoredGenericCounter
                    {
                        BonusPercent = sourcenGenericCounter.BonusPercentage,
                        Inflation = sourcenGenericCounter.Inflation,
                        CurrentSteps = sourcenGenericCounter.CurrentSteps
                    };
                    destinationCouner = destinationGenericCounter;
                }
                var sourceSingleCounter = sourceCounter.Value as SingleCounter;
                if (sourceSingleCounter != null)
                {
                    destinationCouner = new EntityFX.Gdcame.DataAccess.Contract.GameData.StoredSingleCounter();
                }
                var sourceDelayedCounter = sourceCounter.Value as DelayedCounter;
                if (sourceDelayedCounter != null)
                {
                    var destinationDelayedCounter = new EntityFX.Gdcame.DataAccess.Contract.GameData.StoredDelayedCounter()
                    {
                        SecondsRemaining = sourceDelayedCounter.SecondsRemaining,
                        DelayedValue = sourceDelayedCounter.SubValue
                    };
                    destinationCouner = destinationDelayedCounter;
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