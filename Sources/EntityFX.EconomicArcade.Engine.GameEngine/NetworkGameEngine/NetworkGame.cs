using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;

namespace EntityFX.EconomicArcade.Engine.GameEngine.NetworkGameEngine
{
    public class NetworkGame : GameBase
    {
        private static readonly IDictionary<Type, Func<EconomicsArcade.Contract.Common.Counters.CounterBase, CounterBase>> MappingDictionary
            = new ReadOnlyDictionary<Type, Func<EconomicsArcade.Contract.Common.Counters.CounterBase, CounterBase>>(
                new Dictionary<Type, Func<EconomicsArcade.Contract.Common.Counters.CounterBase, CounterBase>>
                    {
                        {
                            typeof(EconomicsArcade.Contract.Common.Counters.GenericCounter),
                            source =>
                            {
                                var sourceGenericCounter =  (EconomicsArcade.Contract.Common.Counters.GenericCounter)source;
                                return new GenericCounter
                                {
                                    BonusPercentage =
                                        sourceGenericCounter.BonusPercentage,
                                    CurrentSteps = sourceGenericCounter.CurrentSteps,
                                    StepsToIncreaseInflation = sourceGenericCounter.InflationIncreaseSteps,
                                    Inflation = sourceGenericCounter.Inflation,
                                    IsUsedInAutoStep =
                                        sourceGenericCounter.UseInAutoSteps
                                };
                            }
                        },
                        {
                            typeof(EconomicsArcade.Contract.Common.Counters.SingleCounter),
                            source =>  new SingleCounter()
                        },
                        {
                            typeof(EconomicsArcade.Contract.Common.Counters.DelayedCounter),
                            source =>
                            {
                                var sourceDelayedCounter =  (EconomicsArcade.Contract.Common.Counters.DelayedCounter)source;
                                return new DelayedCounter
                                {
                                    UnlockValue = sourceDelayedCounter.UnlockValue,
                                    SecondsRemaining = sourceDelayedCounter.SecondsRemaining,
                                    SecondsToAchieve = sourceDelayedCounter.MiningTimeSeconds,
                                    IsMining = sourceDelayedCounter.SecondsRemaining > 0
                                };
                            }
                        }
                    }
            );

        private readonly IGameDataRetrieveDataAccessService _gameDataRetrieveDataAccessService;
        private readonly INotifyGameDataChanged _notifyGameDataChanged;
        private readonly int _userId;
        private GameData _gameData;
        private readonly int _stepsToPersist = 10;
        private int _currentStepsToPersist;

        public NetworkGame(IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService
            , INotifyGameDataChanged notifyGameDataChanged, int userId)
        {
            _gameDataRetrieveDataAccessService = gameDataRetrieveDataAccessService;
            _notifyGameDataChanged = notifyGameDataChanged;
            _userId = userId;
        }

        protected override IDictionary<int, FundsDriver> GetFundsDrivers()
        {
            var result = new Dictionary<int, FundsDriver>();
            foreach (var fundDriver in _gameData.FundsDrivers)
            {
                var incrementors = new Dictionary<int, IncrementorBase>();
                foreach (var incrementor in fundDriver.Incrementors)
                {
                    IncrementorBase value;
                    if (incrementor.Value.IncrementorType == EntityFX.EconomicsArcade.Contract.Common.Incrementors.IncrementorTypeEnum.ValueIncrementor)
                    {
                        value = new ValueIncrementor(incrementor.Value.Value);
                    }
                    else if (incrementor.Value.IncrementorType == EntityFX.EconomicsArcade.Contract.Common.Incrementors.IncrementorTypeEnum.PercentageIncrementor)
                    {
                        value = new PercentageIncrementor(incrementor.Value.Value);
                    }
                    else
                    {
                        value = null;
                    }
                    incrementors.Add(incrementor.Key, value);
                }

                for (int i = 0; i < 3; i++)
                {
                    if (!incrementors.ContainsKey(i))
                    {
                        incrementors.Add(i, new ValueIncrementor(0));
                    }
                }

                result.Add(fundDriver.Id, new FundsDriver()
                {
                    Id = fundDriver.Id,
                    Name = fundDriver.Name,
                    InitialValue = fundDriver.InitialValue,
                    CurrentValue = fundDriver.Value,
                    UnlockValue = fundDriver.UnlockValue,
                    InflationPercent = fundDriver.InflationPercent,
                    BuyCount = fundDriver.BuyCount,
                    Incrementors = incrementors
                });
            }
            return result;
        }

        protected override FundsCounters GetFundsCounters()
        {

            var fundsCounters = _gameData.Counters;
            var counters = new Dictionary<int, CounterBase>();
            var inc = 0;
            foreach (var sourceCounter in fundsCounters.Counters)
            {
                var destinationCouner = MappingDictionary[sourceCounter.GetType()](sourceCounter);
                if (destinationCouner != null)
                {
                    destinationCouner.Name = sourceCounter.Name;
                    destinationCouner.SubValue = sourceCounter.Value;
                    destinationCouner.Id = sourceCounter.Id;
                }
                counters.Add(inc, destinationCouner);
                inc++;
            }
            return new FundsCounters()
            {
                Counters = counters,
                RootCounter = counters[0]
            };
        }



        protected override void PostPerformManualStep(IEnumerable<CounterBase> modifiedCounters)
        {

        }

        private static object _syslock = new { };

        protected override void PostPerformAutoStep(IEnumerable<CounterBase> modifiedCounters)
        {
            lock (_syslock)
            {
                if (_currentStepsToPersist == _stepsToPersist)
                {
                    _currentStepsToPersist = 0;
                    _notifyGameDataChanged.GameDataChanged(this);
                }
                _currentStepsToPersist++;
            }

            _notifyGameDataChanged.AutomaticRefreshed(this);
        }

        protected override void PostBuyFundDriver(FundsDriver fundDriver)
        {
            _notifyGameDataChanged.FundsDriverBought(this, fundDriver);
        }

        protected override void PostInitialize()
        {
            //CashFunds(1500000);
            ManualStepNumber = _gameData.ManualStepsCount;
            AutomaticStepNumber = _gameData.AutomaticStepsCount;
            FundsCounters.CurrentFunds = _gameData.Counters.CurrentFunds;
            FundsCounters.TotalFunds = _gameData.Counters.TotalFunds;
        }

        protected override void PreInitialize()
        {
            //CashFunds(1500000);
            _gameData = _gameDataRetrieveDataAccessService.GetGameData(_userId);
        }

    }

}
