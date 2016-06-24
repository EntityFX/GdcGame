using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicArcade.Engine.GameEngine.UssrSimulator
{
    public  class NotifyGameDataChanged : INotifyGameDataChanged
    {
        private readonly int _userId;
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;

        public NotifyGameDataChanged(int userId, IGameDataStoreDataAccessService gameDataStoreDataAccessService)
        {
            _userId = userId;
            _gameDataStoreDataAccessService = gameDataStoreDataAccessService;
        }

        public void GameDataChanged(IGame game)
        {
            var gameData = PrepareGameDataToPersist(game);
            _gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }

        public void FundsDriverBought(IGame game, EconomicsArcade.Contract.Game.Funds.FundsDriver fundsDriver)
        {
            var gameData = PrepareGameDataToPersist(game, fundsDriver);
            _gameDataStoreDataAccessService.StoreGameDataForUser(_userId, gameData);
        }

        private GameData PrepareGameDataToPersist(IGame game, FundsDriver fundDriver = null)
        {
            return new GameData()
            {
                Counters = new EconomicsArcade.Contract.Common.Counters.FundsCounters()
                {
                    CurrentFunds = game.FundsCounters.CurrentFunds,
                    TotalFunds = game.FundsCounters.TotalFunds,
                    Counters = PrepareCountersToPersist(game)
                },
                FundsDrivers = fundDriver != null ? new[] { PrepareFundDriverToPersist(fundDriver) } : new EconomicsArcade.Contract.Common.Funds.FundsDriver[] { },
                AutomaticStepsCount = game.AutomaticStepNumber,
                ManualStepsCount = game.ManualStepNumber
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
                    var destinationDelayedCounter = new EconomicsArcade.Contract.Common.Counters.DelayedCounter();
                    destinationCouner = destinationDelayedCounter;
                    destinationCouner.Type = 3;
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

        private EconomicsArcade.Contract.Common.Funds.FundsDriver PrepareFundDriverToPersist(FundsDriver fundDriver)
        {
            return new EconomicsArcade.Contract.Common.Funds.FundsDriver()
            {
                Id = fundDriver.Id,
                Name = fundDriver.Name,
                Value = fundDriver.CurrentValue,
                InflationPercent = fundDriver.InflationPercent,
                BuyCount = fundDriver.BuyCount
            };
        }
    }
}