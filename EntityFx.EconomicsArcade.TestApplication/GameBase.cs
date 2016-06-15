using EntityFx.EconomicsArcade.TestApplication.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EntityFx.EconomicsArcade.TestApplication
{
    public abstract class GameBase
    {
        private readonly Timer _stepTimer = new Timer(1000);

        private bool _isInitialized;

        private bool _isStarted;

        public IDictionary<int, FundsDriver> FundsDrivers { get; private set; }

        public FundsCounters FundsCounters { get; private set; }

        public int AutomaticStepNumber { get; private set; }

        public int ManualStepNumber { get; private set; }

        public GameBase()
        {
            _stepTimer.Elapsed += StepTimerElapsed;
        }

        private void StepTimerElapsed(object sender, ElapsedEventArgs e)
        {
            PerformAutoStep();
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
        }

        private void InitializeFundsDrivers()
        {
            FundsDrivers = GetFundsDrivers();
            FundsDrivers.Values.ToList().ForEach(
                _ => {
                    _.CurrentValue = _.InitialValue;
                    _.InflationPercent = 10;
                }
            );
        }

        protected abstract IDictionary<int, FundsDriver> GetFundsDrivers();

        protected abstract FundsCounters GetFundsCounters();

        public void Start()
        {
            if (!_isInitialized) throw new Exception("Game is not initialized");
            _stepTimer.Enabled = true;
            _isStarted = true;
        }

        public void PerformAutoStep()
        {
            if (!_isStarted) throw new Exception("Game is not started");
            PerformAutoStepInternal();
            PostPerformAutoStep();
        }

        private void PerformAutoStepInternal()
        {
            FundsCounters.Counters.Values.OfType<GenericCounter>().ToList().ForEach(
                _ => 
                {
                    if (_.IsUsedInAutoStep)
                    {
                        CashFunds(_.Value);
                    }
                }
            );
            AutomaticStepNumber++;
        }

        private void PerformManualStepInternal()
        {
            FundsCounters.Counters.Values.OfType<GenericCounter>().ToList().ForEach(
                _ =>
                {
                    if (!_.IsUsedInAutoStep)
                    {
                        CashFunds(_.Value);
                    }
                }
            );
            ManualStepNumber++;
        }

        public void PerformManualStep()
        {
            if (!_isStarted) throw new Exception("Game is not started");
            PerformManualStepInternal();
            PostPerformAutoStep();
        }

        public void BuyFundDriver(int fundDriverId)
        {
            if (!FundsDrivers.ContainsKey(fundDriverId))
            {
                return;
            }
            var fundDriver = FundsDrivers[fundDriverId];
            if (FundsCounters.CurrentFunds >= fundDriver.CurrentValue)
            {
                PayWithFunds(fundDriver.CurrentValue);
                fundDriver.CurrentValue = fundDriver.CurrentValue + fundDriver.CurrentValue * fundDriver.InflationPercent / 100.0m;
                fundDriver.BuyCount++;
                IncrementCounters(fundDriver.Incrementors);
            }
            PostByFundDriver(fundDriverId);
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
                            counter.Value.Value += incrementors[counter.Key].Value;
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
    }
}
