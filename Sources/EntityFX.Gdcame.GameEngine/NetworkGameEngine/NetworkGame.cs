﻿using System;
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
using CounterBase = EntityFX.Gdcame.Common.Contract.Counters.CounterBase;
using DelayedCounter = EntityFX.Gdcame.Common.Contract.Counters.DelayedCounter;
using GenericCounter = EntityFX.Gdcame.Common.Contract.Counters.GenericCounter;
using IncrementorTypeEnum = EntityFX.Gdcame.Common.Contract.Incrementors.IncrementorTypeEnum;
using SingleCounter = EntityFX.Gdcame.Common.Contract.Counters.SingleCounter;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public class NetworkGame : GameBase
    {
        private static readonly IDictionary<Type, Func<CounterBase, Contract.Counters.CounterBase>> MappingDictionary
            = new ReadOnlyDictionary<Type, Func<CounterBase, Contract.Counters.CounterBase>>(
                new Dictionary<Type, Func<CounterBase, Contract.Counters.CounterBase>>
                {
                    {
                        typeof (GenericCounter),
                        source =>
                        {
                            var sourceGenericCounter = (GenericCounter) source;
                            var res = new Contract.Counters.GenericCounter
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
                        typeof (SingleCounter),
                        source => new Contract.Counters.SingleCounter
                        {
                            SubValue = source.Value
                        }
                    },
                    {
                        typeof (DelayedCounter),
                        source =>
                        {
                            var sourceDelayedCounter = (DelayedCounter) source;
                            return new Contract.Counters.DelayedCounter
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

        private static object _syslock = new {};
        private readonly IGameDataRetrieveDataAccessService _gameDataRetrieveDataAccessService;

        private readonly ILogger _logger;
        private readonly IGameDataChangesNotifier _gameDataChangesNotifier;
        private readonly string _userId;
        private GameData _gameData;

        public NetworkGame(ILogger logger, IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService
            , IGameDataChangesNotifier gameDataChangesNotifier, string userId)
        {
            _logger = logger;
            _gameDataRetrieveDataAccessService = gameDataRetrieveDataAccessService;
            _gameDataChangesNotifier = gameDataChangesNotifier;
            _userId = userId;
        }

        public int StepsToPersist { get; private set; }

        protected override ReadOnlyDictionary<int, Item> GetFundsDrivers()
        {
            var result = new Dictionary<int, Item>();
            foreach (var item in _gameData.Items)
            {
                var incrementors = new Dictionary<int, IncrementorBase>();
                foreach (var incrementor in item.Incrementors)
                {
                    IncrementorBase value;
                    if (incrementor.Value.IncrementorType == IncrementorTypeEnum.ValueIncrementor)
                    {
                        value = IncrementorFactory.Build<ValueIncrementor>(incrementor.Value.Value);
                    }
                    else if (incrementor.Value.IncrementorType == IncrementorTypeEnum.PercentageIncrementor)
                    {
                        value = IncrementorFactory.Build<PercentageIncrementor>(incrementor.Value.Value);
                    }
                    else
                    {
                        value = null;
                    }
                    incrementors.Add(incrementor.Key, value);
                }

                result.Add(item.Id, new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    InitialPrice = item.InitialValue,
                    Price = item.Price,
                    UnlockBalance = item.UnlockValue,
                    InflationPercent = item.InflationPercent,
                    Bought = item.Bought,
                    Incrementors = incrementors,
                    CustomRuleInfo = item.CustomRuleInfo != null
                        ? new CustomRuleInfo
                        {
                            CustomRule = CustomRules[item.CustomRuleInfo.CustomRuleId],
                            CurrentIndex = item.CustomRuleInfo.CurrentIndex
                        }
                        : null
                });
            }
            return new ReadOnlyDictionary<int, Item>(result);
        }

        protected override GameCash GetFundsCounters()
        {
            var fundsCounters = _gameData.Cash;
            var counters = new Dictionary<int, Contract.Counters.CounterBase>();
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
            return new GameCash
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

        protected override void PostPerformManualStep(IEnumerable<Contract.Counters.CounterBase> modifiedCounters)
        {
        }

        public void PerformGameDataChanged()
        {
            ///////_gameDataChangesNotifier.GameDataChanged(this);
            //throw new NotImplementedException("commented out previous line!!!");
        }

        protected override void PostPerformAutoStep(IEnumerable<Contract.Counters.CounterBase> modifiedCounters,
            int iterations)
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

            _gameDataChangesNotifier.AutomaticRefreshed(this);
        }

        protected override void PostBuyFundDriver(Item fundDriver)
        {
            ///////_gameDataChangesNotifier.FundsDriverBought(this, fundDriver);
            //throw new NotImplementedException("commented out previous line!!!");
        }

        protected override void PostInitialize()
        {
            //CashFunds(1500000);
            ManualStepNumber = _gameData.ManualStepsCount;
            AutomaticStepNumber = _gameData.AutomatedStepsCount;
            GameCash.CashOnHand = _gameData.Cash.OnHand;
            GameCash.TotalEarned = _gameData.Cash.Total;
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