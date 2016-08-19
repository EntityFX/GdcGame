using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;
using IncrementorTypeEnum = EntityFX.Gdcame.Common.Contract.Incrementors.IncrementorTypeEnum;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public class NetworkGame : GameBase
    {
        private static readonly IDictionary<Type, Func<Gdcame.Common.Contract.Counters.CounterBase, CounterBase>> MappingDictionary
            = new ReadOnlyDictionary<Type, Func<Gdcame.Common.Contract.Counters.CounterBase, CounterBase>>(
                new Dictionary<Type, Func<Gdcame.Common.Contract.Counters.CounterBase, CounterBase>>
                    {
                        {
                            typeof(Gdcame.Common.Contract.Counters.GenericCounter),
                            source =>
                            {
                                var sourceGenericCounter =  (Gdcame.Common.Contract.Counters.GenericCounter)source;
                                var res = new GenericCounter
                                {
                                    BonusPercentage =
                                        sourceGenericCounter.BonusPercentage,
                                    StepsToIncreaseInflation = sourceGenericCounter.InflationIncreaseSteps,
                                    Inflation = sourceGenericCounter.Inflation,
                                    SubValue = sourceGenericCounter.SubValue,
                                    IsUsedInAutoStep =
                                        sourceGenericCounter.UseInAutoSteps
                                };
                                res.CurrentSteps = sourceGenericCounter.CurrentSteps;
                                return res;
                            }
                        },
                        {
                            typeof(Gdcame.Common.Contract.Counters.SingleCounter),
                            source =>  new SingleCounter 
                            {
                                                    SubValue = source.Value
                            }
                        },
                        {
                            typeof(Gdcame.Common.Contract.Counters.DelayedCounter),
                            source =>
                            {
                                var sourceDelayedCounter =  (Gdcame.Common.Contract.Counters.DelayedCounter)source;
                                return new DelayedCounter
                                {
                                    UnlockValue = sourceDelayedCounter.UnlockValue,
                                    SubValue = sourceDelayedCounter.Value,
                                    SecondsRemaining = sourceDelayedCounter.SecondsRemaining,
                                    SecondsToAchieve = sourceDelayedCounter.MiningTimeSeconds,
                                    IsMining = sourceDelayedCounter.SecondsRemaining > 0
                                };
                            }
                        }
                    }
            );

        private readonly ILogger _logger;
        private readonly IGameDataRetrieveDataAccessService _gameDataRetrieveDataAccessService;
        private readonly INotifyGameDataChanged _notifyGameDataChanged;
        private readonly int _userId;
        private GameData _gameData;

        public NetworkGame(ILogger logger, IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService
            , INotifyGameDataChanged notifyGameDataChanged, int userId)
        {
            _logger = logger;
            _gameDataRetrieveDataAccessService = gameDataRetrieveDataAccessService;
            _notifyGameDataChanged = notifyGameDataChanged;
            _userId = userId;
        }

        protected override ReadOnlyDictionary<int, Item> GetFundsDrivers()
        {
            var result = new Dictionary<int, Item>();
            foreach (var fundDriver in _gameData.Items)
            {
                var incrementors = new Dictionary<int, IncrementorBase>();
                foreach (var incrementor in fundDriver.Incrementors)
                {
                    IncrementorBase value;
                    if (incrementor.Value.IncrementorType == IncrementorTypeEnum.ValueIncrementor)
                    {
                        value = new ValueIncrementor(incrementor.Value.Value);
                    }
                    else if (incrementor.Value.IncrementorType == IncrementorTypeEnum.PercentageIncrementor)
                    {
                        value = new PercentageIncrementor(incrementor.Value.Value);
                    }
                    else
                    {
                        value = null;
                    }
                    incrementors.Add(incrementor.Key, value);
                }

                result.Add(fundDriver.Id, new Item()
                {
                    Id = fundDriver.Id,
                    Name = fundDriver.Name,
                    InitialValue = fundDriver.InitialValue,
                    CurrentValue = fundDriver.Price,
                    UnlockValue = fundDriver.UnlockValue,
                    InflationPercent = fundDriver.InflationPercent,
                    BuyCount = fundDriver.BuyCount,
                    Incrementors = incrementors,
                    CustomRuleInfo = fundDriver.CustomRuleInfo != null ? new CustomRuleInfo()
                    {
                        CustomRule = CustomRules[fundDriver.CustomRuleInfo.CustomRuleId],
                        CurrentIndex = fundDriver.CustomRuleInfo.CurrentIndex
                    } : null
                });
            }
            return new ReadOnlyDictionary<int, Item>(result);
        }

        protected override GameCash GetFundsCounters()
        {

            var fundsCounters = _gameData.Cash;
            var counters = new Dictionary<int, CounterBase>();
            var inc = 0;
            foreach (var sourceCounter in fundsCounters.Counters)
            {
                var destinationCouner = MappingDictionary[sourceCounter.GetType()](sourceCounter);
                if (destinationCouner != null)
                {
                    destinationCouner.Name = sourceCounter.Name;
                    destinationCouner.Id = sourceCounter.Id;
                }
                counters.Add(inc, destinationCouner);
                inc++;
            }
            return new GameCash()
            {
                Counters = counters,
                RootCounter = counters[0]
            };
        }

        protected override ReadOnlyDictionary<int, ICustomRule> GetCustomRules()
        {
            var customRuleDictionary = new Dictionary<int, ICustomRule>();
            foreach (var customRule in _gameData.CustomRules)
            {
                var customRuleInstance = (ICustomRule) Activator.CreateInstance(
                    Type.GetType(string.Format("EntityFX.Gdcame.GameEngine.CustomRules.{0}", customRule.Name)));
                customRuleInstance.Id = customRule.Id;
                customRuleDictionary.Add(customRule.Id, customRuleInstance
                    );
            }
            return new ReadOnlyDictionary<int, ICustomRule>(customRuleDictionary);
        }

        protected override void PostPerformManualStep(IEnumerable<CounterBase> modifiedCounters)
        {

        }

        private static object _syslock = new { };

        public int StepsToPersist
        {
            get;
        }

        public void PerformGameDataChanged()
        {
            _notifyGameDataChanged.GameDataChanged(this);
        }

        protected override void PostPerformAutoStep(IEnumerable<CounterBase> modifiedCounters, int iterations)
        {
            /*lock (_syslock)
            {   */
                //if (_currentStepsToPersist == _stepsToPersist)
                //{
                //    _currentStepsToPersist = 0;
                //    _notifyGameDataChanged.GameDataChanged(this);
                //}
                //_currentStepsToPersist++;
            /*}  */

            _notifyGameDataChanged.AutomaticRefreshed(this);
        }

        protected override void PostBuyFundDriver(Item fundDriver)
        {
            _notifyGameDataChanged.FundsDriverBought(this, fundDriver);
        }

        protected override void PostInitialize()
        {
            //CashFunds(1500000);
            ManualStepNumber = _gameData.ManualStepsCount;
            AutomaticStepNumber = _gameData.AutomatedStepsCount;
            GameCash.CashOnHand = _gameData.Cash.CashOnHand;
            GameCash.TotalEarned = _gameData.Cash.TotalEarned;
        }

        protected override void PreInitialize()
        {
            //CashFunds(1500000);
            var sw = new Stopwatch();
            sw.Start();
            _gameData = _gameDataRetrieveDataAccessService.GetGameData(_userId);
            _logger.Info("Perform PreInitialize: {0}", sw.Elapsed);
        }

    }

}
