﻿using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Counters;

    public class StoreGameDataMapper : IMapper<IGame, StoredGameData>
    {
        public StoredGameData Map(IGame source, StoredGameData destination = null)
        {
            var gameData = new StoredGameData
            {
                Cash = new StoredCash
                {
                    Balance = source.GameCash.CashOnHand,
                    TotalEarned = source.GameCash.TotalEarned,
                    Counters = PrepareCountersToPersist(source)
                },
                AutomatedStepsCount = source.AutomaticStepNumber,
                ManualStepsCount = source.ManualStepNumber
            };
            return gameData;
        }

        private StoredCounterBase[] PrepareCountersToPersist(IGame game)
        {
            var counters = new StoredCounterBase[game.GameCash.Counters.Length];
            foreach (var sourceCounter in game.GameCash.Counters)
            {
                StoredCounterBase destinationCouner = null;
                var sourcenGenericCounter = sourceCounter as GenericCounter;
                if (sourcenGenericCounter != null)
                {
                    var destinationGenericCounter = new StoredGenericCounter
                    {
                        BonusPercent = sourcenGenericCounter.BonusPercentage,
                        Inflation = sourcenGenericCounter.Inflation,
                        CurrentSteps = sourcenGenericCounter.CurrentSteps
                    };
                    destinationCouner = destinationGenericCounter;
                }
                var sourceSingleCounter = sourceCounter as SingleCounter;
                if (sourceSingleCounter != null)
                {
                    destinationCouner = new StoredSingleCounter();
                }
                var sourceDelayedCounter = sourceCounter as DelayedCounter;
                if (sourceDelayedCounter != null)
                {
                    var destinationDelayedCounter = new StoredDelayedCounter
                    {
                        SecondsRemaining = sourceDelayedCounter.SecondsRemaining,
                        DelayedValue = sourceDelayedCounter.SubValue
                    };
                    destinationCouner = destinationDelayedCounter;
                }
                if (destinationCouner != null)
                {
                    destinationCouner.Id = sourceCounter.Id;
                    destinationCouner.Value = sourceCounter.Value;
                }
                if (destinationCouner != null) counters[destinationCouner.Id] = destinationCouner;
            }
            return counters;
        }
    }
}