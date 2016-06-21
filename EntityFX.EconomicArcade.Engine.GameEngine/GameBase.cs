using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;

namespace EntityFX.EconomicArcade.Engine.GameEngine
{
    public abstract class GameBase : IGame
    {
        private bool _isInitialized;

        public IDictionary<int, FundsDriver> FundsDrivers { get; private set; }

        public FundsCounters FundsCounters { get; private set; }

        public int AutomaticStepNumber { get; private set; }

        public int ManualStepNumber { get; private set; }

        protected GameBase()
        {
        }

        public void Initialize()
        {
            InitializeFundsCounters();
            InitializeFundsDrivers();
            PostInitialize();
            _isInitialized = true;
        }

        private void InitializeFundsCounters()
        {
            FundsCounters = GetFundsCounters();
            var genericCounters = FundsCounters.Counters.Values.OfType<GenericCounter>();
            foreach (var fundCounter in genericCounters)
            {
                fundCounter.StepsToIncreaseInflation = fundCounter.StepsToIncreaseInflation == 0 ? 1000 : fundCounter.StepsToIncreaseInflation;
            }
        }

        private void InitializeFundsDrivers()
        {
            FundsDrivers = GetFundsDrivers();
            foreach (var fundDriver in FundsDrivers)
            {
                fundDriver.Value.CurrentValue = fundDriver.Value.InitialValue;
                fundDriver.Value.InflationPercent = 15;
            }
        }

        protected abstract IDictionary<int, FundsDriver> GetFundsDrivers();

        protected abstract FundsCounters GetFundsCounters();

        public async Task<int> PerformAutoStep()
        {
            if (!_isInitialized) throw new Exception("Game is not started");
            return await Task<int>.Factory.StartNew(() =>
            {
                PerformAutoStepInternal();
                PostPerformAutoStep();
                return 0;
            });
        }

        private void PerformAutoStepInternal()
        {
            var genericCounters = FundsCounters.Counters.Values.OfType<GenericCounter>()
                .Where(_ => _.IsUsedInAutoStep);
            foreach (var genericCounter in genericCounters)
            {
                CashFunds(genericCounter.Value);
                genericCounter.CurrentSteps++;
                genericCounter.Inflation = genericCounter.CurrentSteps / genericCounter.StepsToIncreaseInflation;
            }

            var genericManualCounters = FundsCounters.Counters.Values.OfType<GenericCounter>()
                .Where(_ => !_.IsUsedInAutoStep);
            foreach (var manualCounter in genericManualCounters)
            {
                manualCounter.CurrentSteps--;
                manualCounter.Inflation = manualCounter.CurrentSteps / manualCounter.StepsToIncreaseInflation;
            }
            AutomaticStepNumber++;
        }

        private void PerformManualStepInternal()
        {
            var genericCounters = FundsCounters.Counters.Values.OfType<GenericCounter>()
                .Where(_ => !_.IsUsedInAutoStep);
            foreach (var genericCounter in genericCounters)
            {
                CashFunds(genericCounter.Value);
                genericCounter.CurrentSteps++;
                genericCounter.Inflation = genericCounter.CurrentSteps / genericCounter.StepsToIncreaseInflation;
            }
            ManualStepNumber++;
        }

        public void PerformManualStep()
        {
            if (!_isInitialized) throw new Exception("Game is not started");

                PerformManualStepInternal();
                PostPerformManualStep();

        }

        public void BuyFundDriver(int fundDriverId)
        {
            if (!FundsDrivers.ContainsKey(fundDriverId))
            {
                return;
            }
            var fundDriver = FundsDrivers[fundDriverId];
            if (!IsFundsDriverAvailableForBuy(fundDriver))
            {
                return;
            }
            if (FundsCounters.CurrentFunds >= fundDriver.CurrentValue)
            {
                PayWithFunds(fundDriver.CurrentValue);
                fundDriver.CurrentValue = fundDriver.CurrentValue + fundDriver.CurrentValue * fundDriver.InflationPercent / 100.0m;
                fundDriver.BuyCount++;
                IncrementCounters(fundDriver.Incrementors);
            }
            PostByFundDriver(fundDriverId);
        }

        protected virtual void PostPerformManualStep()
        {

        }

        protected virtual void PostPerformAutoStep()
        {

        }

        protected virtual void PostByFundDriver(int fundDriverId)
        {

        }

        protected virtual void PostInitialize()
        {

        }

        protected void CashFunds(decimal value)
        {
            FundsCounters.CurrentFunds += value;
            FundsCounters.TotalFunds += value;
        }

        protected void PayWithFunds(decimal value)
        {
            FundsCounters.CurrentFunds -= value;
        }

        protected void IncrementCounters(IDictionary<int, IncrementorBase> incrementors)
        {
            var incrementorsList = incrementors.ToList(); 
            foreach (var counter in  FundsCounters.Counters)
	        {
                IncrementorBase incrementor;
                incrementors.TryGetValue(counter.Key, out incrementor);
                if (incrementor != null)
                {
                    if (incrementor.IncrementorType == IncrementorTypeEnum.ValueIncrementor)
                    {
                        var genericCounter = counter.Value as GenericCounter;
                        if (genericCounter != null)
                        {
                            genericCounter.SubValue += incrementor.Value;
                        }
                        else
                        {
                            counter.Value.SubValue += incrementors[counter.Key].Value;
                        }

                    }
                    else if (incrementor.IncrementorType == IncrementorTypeEnum.PercentageIncrementor)
                    {
                        var genericCounter = counter.Value as GenericCounter;
                        if (genericCounter != null) 
                        {
                            genericCounter.BonusPercentage += incrementor.Value;
                        }
                    }
                }
	        }
        }

        protected string GetIncrementorValueById(FundsDriver fundsDriver, int incrmentorId)
        {
            var incrementor = fundsDriver.Incrementors.ContainsKey(incrmentorId) ?
                fundsDriver.Incrementors[incrmentorId] : null;
            if (incrementor != null)
            {
                return String.Format("{0}{1}", incrementor.Value, incrementor.IncrementorType == IncrementorTypeEnum.PercentageIncrementor ? "%" : string.Empty);
            }
            return "0";
        }

        protected bool IsFundsDriverAvailableForBuy(FundsDriver fundsDriver)
        {
            return fundsDriver.UnlockValue <= FundsCounters.RootCounter.Value;
        }


        public void FightAgainstInflation()
        {
            var genericCounters = FundsCounters.Counters.Values.OfType<GenericCounter>()
                .Where(_ => _.IsUsedInAutoStep);
            var enumerable = genericCounters as IList<GenericCounter> ?? genericCounters.ToList();
            if (enumerable.All(_ => _.Inflation == 0)) 
            {
                return;
            }

            foreach (var genericCounter in enumerable)
            {
                genericCounter.CurrentSteps = 0;
                genericCounter.Inflation = genericCounter.CurrentSteps / genericCounter.StepsToIncreaseInflation;
            }
        }

        public LotteryResult PlayLottery()
        {
            throw new NotImplementedException();
        }
    }
}
