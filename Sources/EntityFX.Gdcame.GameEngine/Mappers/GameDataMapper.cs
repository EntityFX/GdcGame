namespace EntityFX.Gdcame.Engine.GameEngine.Mappers
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;

    using DelayedCounter = EntityFX.Gdcame.Kernel.Contract.Counters.DelayedCounter;
    using GenericCounter = EntityFX.Gdcame.Kernel.Contract.Counters.GenericCounter;
    using SingleCounter = EntityFX.Gdcame.Kernel.Contract.Counters.SingleCounter;

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
                    Counters = this.PrepareCountersToPersist(source)
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
                    var destinationGenericCounter = new Gdcame.Contract.MainServer.Counters.GenericCounter
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
                    destinationCouner = new Gdcame.Contract.MainServer.Counters.SingleCounter();
                    destinationCouner.Type = 0;
                }
                var sourceDelayedCounter = sourceCounter as DelayedCounter;
                if (sourceDelayedCounter != null)
                {
                    var destinationDelayedCounter = new Gdcame.Contract.MainServer.Counters.DelayedCounter
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