using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Presentation.Shared.KernelConsole
{
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;

    using DelayedCounter = EntityFX.Gdcame.Kernel.Contract.Counters.DelayedCounter;
    using GenericCounter = EntityFX.Gdcame.Kernel.Contract.Counters.GenericCounter;
    using SingleCounter = EntityFX.Gdcame.Kernel.Contract.Counters.SingleCounter;

    public class GameDataMapper : IMapper<IGame, GameDataModel>
    {
        public GameDataModel Map(IGame source, GameDataModel destination = null)
        {
            var gameData = new GameDataModel
            {
                Cash = new CashModel
                {
                    OnHand = source.GameCash.CashOnHand,
                    TotalEarned = source.GameCash.TotalEarned,
                    Counters = this.PrepareCountersToPersist(source)
                }
            };
            return gameData;
        }

        private CounterModelBase[] PrepareCountersToPersist(IGame game)
        {
            var counters = new CounterModelBase[game.GameCash.Counters.Length];
            foreach (var sourceCounter in game.GameCash.Counters)
            {
                CounterModelBase destinationCouner = null;
                var sourcenGenericCounter = sourceCounter as GenericCounter;
                if (sourcenGenericCounter != null)
                {
                    var destinationGenericCounter = new GenericCounterModel
                    {
                        BonusPercentage = sourcenGenericCounter.BonusPercentage,
                        Bonus = sourcenGenericCounter.Bonus,
                        Inflation = sourcenGenericCounter.Inflation,
                        SubValue = sourcenGenericCounter.SubValue
                    };
                    destinationCouner = destinationGenericCounter;
                    destinationCouner.Type = 1;
                }
                var sourceSingleCounter = sourceCounter as SingleCounter;
                if (sourceSingleCounter != null)
                {
                    destinationCouner = new SingleCounterModel();
                    destinationCouner.Type = 0;
                }
                var sourceDelayedCounter = sourceCounter as DelayedCounter;
                if (sourceDelayedCounter != null)
                {
                    var destinationDelayedCounter = new DelayedCounterModel
                    {
                        SecondsRemaining = sourceDelayedCounter.SecondsRemaining,
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