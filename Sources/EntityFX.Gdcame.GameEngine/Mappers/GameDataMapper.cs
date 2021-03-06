﻿using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.Infrastructure.Common;
using DelayedCounter = EntityFX.Gdcame.GameEngine.Contract.Counters.DelayedCounter;
using GenericCounter = EntityFX.Gdcame.GameEngine.Contract.Counters.GenericCounter;
using SingleCounter = EntityFX.Gdcame.GameEngine.Contract.Counters.SingleCounter;

namespace EntityFX.Gdcame.GameEngine.Mappers
{
    public class GameDataMapper : IMapper<IGame, GameData>
    {
        public GameData Map(IGame source, GameData destination = null)
        {
            var gameData = new GameData
            {
                Cash = new Cash
                {
                    OnHand = source.GameCash.CashOnHand,
                    Total = source.GameCash.TotalEarned,
                    Counters = PrepareCountersToPersist(source)
                },
                AutomatedStepsCount = source.AutomaticStepNumber,
                ManualStepsCount = source.ManualStepNumber
            };
            return gameData;
        }

        private CounterBase[] PrepareCountersToPersist(IGame game)
        {
            var counters = new CounterBase[game.GameCash.Counters.Length];
            foreach (var sourceCounter in game.GameCash.Counters)
            {
                CounterBase destinationCouner = null;
                var sourcenGenericCounter = sourceCounter as GenericCounter;
                if (sourcenGenericCounter != null)
                {
                    var destinationGenericCounter = new Common.Contract.Counters.GenericCounter
                    {
                        BonusPercentage = sourcenGenericCounter.BonusPercentage,
                        Bonus = sourcenGenericCounter.Bonus,
                        Inflation = sourcenGenericCounter.Inflation,
                        CurrentSteps = sourcenGenericCounter.CurrentSteps,
                        SubValue = sourcenGenericCounter.SubValue
                    };
                    destinationCouner = destinationGenericCounter;
                    destinationCouner.Type = 1;
                }
                var sourceSingleCounter = sourceCounter as SingleCounter;
                if (sourceSingleCounter != null)
                {
                    destinationCouner = new Common.Contract.Counters.SingleCounter();
                    destinationCouner.Type = 0;
                }
                var sourceDelayedCounter = sourceCounter as DelayedCounter;
                if (sourceDelayedCounter != null)
                {
                    var destinationDelayedCounter = new Common.Contract.Counters.DelayedCounter
                    {
                        SecondsRemaining = sourceDelayedCounter.SecondsRemaining,
                        MiningTimeSeconds = sourceDelayedCounter.SecondsToAchieve,
                        UnlockValue = sourceDelayedCounter.UnlockValue
                    };
                    destinationCouner = destinationDelayedCounter;
                    destinationCouner.Type = 2;
                }
                if (destinationCouner != null)
                {
                    destinationCouner.Name = sourceCounter.Name;
                    destinationCouner.Id = sourceCounter.Id;
                    destinationCouner.Value = sourceCounter.Value;
                }
                if (destinationCouner != null) counters[destinationCouner.Id] = destinationCouner;
            }
            return counters;
        }
    }
}