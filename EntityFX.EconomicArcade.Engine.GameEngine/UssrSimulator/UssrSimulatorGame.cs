using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;

namespace EntityFX.EconomicArcade.Engine.GameEngine.UssrSimulator
{
    public class UssrSimulatorGame : GameBase
    {
        private readonly IGameDataRetrieveDataAccessService _gameDataRetrieveDataAccessService;
        private readonly INotifyGameDataChanged _notifyGameDataChanged;
        private readonly int _userId;
        private GameData _gameData;
        private readonly int _stepsToPersist = 10;
        private int _currentStepsToPersist;

        public UssrSimulatorGame(IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService
            , INotifyGameDataChanged notifyGameDataChanged, int userId)
        {
            _gameDataRetrieveDataAccessService = gameDataRetrieveDataAccessService;
            _notifyGameDataChanged = notifyGameDataChanged;
            _userId = userId;
        }

        public decimal Communism
        {
            get
            {
                return FundsCounters.Counters[(int)UssrCounterEnum.Communism].SubValue;
            }
        }

        public decimal Production
        {
            get
            {
                return FundsCounters.Counters[(int)UssrCounterEnum.Production].SubValue;
            }
        }

        public decimal Taxes
        {
            get
            {
                return FundsCounters.Counters[(int)UssrCounterEnum.Tax].SubValue;
            }
        }

        public decimal FiveYearPlan
        {
            get
            {
                return FundsCounters.Counters[(int)UssrCounterEnum.FiveYearPlan].SubValue;
            }
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
                CounterBase destinationCouner = null;
                if (sourceCounter.GetType() == typeof(EntityFX.EconomicsArcade.Contract.Common.Counters.GenericCounter))
                {
                    var destinationGenericCounter = new GenericCounter();
                    var sourceGenericCounter =  (EconomicsArcade.Contract.Common.Counters.GenericCounter)sourceCounter;
                    destinationGenericCounter.BonusPercentage = sourceGenericCounter.BonusPercentage;
                    destinationGenericCounter.CurrentSteps = sourceGenericCounter.CurrentSteps;
                    destinationGenericCounter.Inflation = sourceGenericCounter.Inflation;
                    destinationGenericCounter.IsUsedInAutoStep = sourceGenericCounter.UseInAutoSteps;
                    destinationCouner = destinationGenericCounter;
                }
                if (sourceCounter.GetType() == typeof(EntityFX.EconomicsArcade.Contract.Common.Counters.SingleCounter))
                {
                    destinationCouner = new SingleCounter();
                }
                if (sourceCounter.GetType() == typeof(EntityFX.EconomicsArcade.Contract.Common.Counters.DelayedCounter))
                {
                    destinationCouner = new DelayedCounter();
                }
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

        private static object _syslock = new {};

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
