using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Common;
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

        public int AutomaticStepNumber { get; protected set; }

        public int ManualStepNumber { get; protected set; }

        private int _nextVerificationSteps = 0;

        private int _currentVerificationSteps = 0;
#if DEBUG 
        private readonly int _stepsToVerify = 500;
#else
        private readonly int _stepsToVerify = 700;
#endif

        private ManualStepResult _manualStepVerificationRequiredResult;

        private readonly Random _randomiserForVerification = new Random(Environment.TickCount);

        protected GameBase()
        {
        }

        public void Initialize()
        {
            PreInitialize();
            InitializeFundsCounters();
            InitializeFundsDrivers();
            InitializeVerificationSteps();
            PostInitialize();
            _isInitialized = true;
        }

        private ManualStepVerificationRequiredResult InitializeVerificationSteps()
        {
            var verificationManualStepResult = new ManualStepVerificationRequiredResult
            {
                FirstNumber = _randomiserForVerification.Next(1, 50),
                SecondNumber = _randomiserForVerification.Next(1, 50),
            };
            _currentVerificationSteps = 0;
            var delta = (int)(_stepsToVerify * 0.1);
            _nextVerificationSteps = _stepsToVerify + _randomiserForVerification.Next(-1 * delta, delta);
            return verificationManualStepResult;
        }

        protected virtual void PreInitialize()
        {

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
                if (fundDriver.Value.CurrentValue == 0)
                {
                    fundDriver.Value.CurrentValue = fundDriver.Value.InitialValue;
                }
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
                var modifiedCounters = PerformAutoStepInternal();
                PostPerformAutoStep(modifiedCounters);
                return 0;
            });
        }

        private IEnumerable<CounterBase> PerformAutoStepInternal()
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

            var delayedCounters = FundsCounters.Counters.Values.OfType<DelayedCounter>()
                .Where(_ => _.IsMining);
            foreach (var delayedCounter in delayedCounters)
            {
                if (delayedCounter.SecondsRemaining == 0)
                {
                    delayedCounter.IsMining = false;
                    CashFunds(delayedCounter.Value);
                    delayedCounter.SubValue *= 1.2m;
                }
                else
                {
                    delayedCounter.SecondsRemaining--;
                }
            }

            AutomaticStepNumber++;
            return genericCounters;
        }

        private IEnumerable<CounterBase> PerformManualStepInternal()
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
            return genericCounters;
        }

        private ManualStepResult VerifyManualStep(VerificationManualStepData verificationData)
        {
            if (_manualStepVerificationRequiredResult != null)
            {
                if (verificationData == null)
                {
                    return _manualStepVerificationRequiredResult;
                }
                var verficationRequiredResult =
                    _manualStepVerificationRequiredResult as ManualStepVerificationRequiredResult;

                ManualStepResult result = verficationRequiredResult != null && verficationRequiredResult.FirstNumber + verficationRequiredResult.SecondNumber ==
                                          verificationData.ResultNumber ? (ManualStepResult) new ManualStepVerifiedResult(true) : InitializeVerificationSteps();



                if (!result.IsVerificationRequired)
                {
                    _manualStepVerificationRequiredResult = null;
                }
                else
                {
                    _manualStepVerificationRequiredResult = result;
                }
                return result;
            }

            if (_currentVerificationSteps < _nextVerificationSteps)
            {
                _currentVerificationSteps++;
            }
            else
            {
                _manualStepVerificationRequiredResult = InitializeVerificationSteps();
                return _manualStepVerificationRequiredResult;
            }

            return new ManualStepNoVerficationRequiredResult();
        }

        public ManualStepResult PerformManualStep(VerificationManualStepData verificationData)
        {
            if (!_isInitialized) throw new InvalidOperationException("Game is not started");

            var verificationResult = VerifyManualStep(verificationData);
            IEnumerable<CounterBase> modifiedCounters = null;

            if (!verificationResult.IsVerificationRequired)
            {
                modifiedCounters = PerformManualStepInternal();
                PostPerformManualStep(modifiedCounters);
            }
            return verificationResult;
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
            PostBuyFundDriver(fundDriver);
        }

        protected virtual void PostPerformManualStep(IEnumerable<CounterBase> modifiedCounters)
        {

        }

        protected virtual void PostPerformAutoStep(IEnumerable<CounterBase> modifiedCounters)
        {

        }

        protected virtual void PostBuyFundDriver(FundsDriver fundDriver)
        {

        }

        protected virtual void PostInitialize()
        {

        }

        protected virtual void CashFunds(decimal value)
        {
            FundsCounters.CurrentFunds += value;
            FundsCounters.TotalFunds += value;
        }

        protected virtual void PayWithFunds(decimal value)
        {
            FundsCounters.CurrentFunds -= value;
        }

        protected virtual void IncrementCounters(IDictionary<int, IncrementorBase> incrementors)
        {
            var incrementorsList = incrementors.ToList();
            foreach (var counter in FundsCounters.Counters)
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
            FundsCounters.RootCounter.SubValue += 1;
        }

        public void ActivateDelayedCounter(int counterId)
        {
            var delayedCounter = (DelayedCounter)FundsCounters.Counters[counterId];
            if (delayedCounter == null)
            {
                return;
            }
            if (delayedCounter.IsMining | delayedCounter.UnlockValue > FundsCounters.RootCounter.Value)
            {
                return;
            }
            delayedCounter.IsMining = true;
            delayedCounter.SecondsRemaining = delayedCounter.SecondsToAchieve;
        }

        public LotteryResult PlayLottery()
        {
            throw new NotImplementedException();
        }
    }
}
