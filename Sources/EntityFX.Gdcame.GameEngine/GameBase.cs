using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;

namespace EntityFX.Gdcame.GameEngine
{
    public abstract class GameBase : IGame
    {
        private bool _isInitialized;

        public ReadOnlyDictionary<int, Item> Items { get; private set; }

        public Dictionary<int, Item> ModifiedFundsDrivers { get; }

        public GameCash GameCash { get; private set; }

        public ReadOnlyDictionary<int, ICustomRule> CustomRules { get; private set; }

        public int AutomaticStepNumber { get; protected set; }

        public int ManualStepNumber { get; protected set; }

        private int _nextVerificationSteps;

        private int _currentVerificationSteps;
#if DEBUG
        private readonly int _stepsToVerify = 200;
#else
        private readonly int _stepsToVerify = 200;
#endif

        private ManualStepResult _manualStepVerificationRequiredResult;

        private readonly Random _randomiserForVerification = new Random(Environment.TickCount);

        private static readonly object _lockState = new {};
        private IEnumerable<GenericCounter> _autoGenericCounters;
        private IEnumerable<GenericCounter> _manualGenericCounters;
        private IEnumerable<DelayedCounter> _delayedCounters;

        protected GameBase()
        {
            ModifiedFundsDrivers = new Dictionary<int, Item>();
        }

        public void Initialize()
        {
            PreInitialize();
            InitializeFundsCounters();
            InitializeCustomRules();
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
                SecondNumber = _randomiserForVerification.Next(1, 50)
            };
            _currentVerificationSteps = 0;
            var delta = (int) (_stepsToVerify*0.1);
            _nextVerificationSteps = _stepsToVerify + _randomiserForVerification.Next(-1*delta, delta);
            return verificationManualStepResult;
        }

        protected virtual void PreInitialize()
        {
        }

        private void InitializeFundsCounters()
        {
            GameCash = GetFundsCounters();
            var genericCounters = GameCash.Counters.Values.OfType<GenericCounter>().ToArray();
            foreach (var fundCounter in genericCounters)
            {
                fundCounter.StepsToIncreaseInflation = fundCounter.StepsToIncreaseInflation == 0
                    ? 1000
                    : fundCounter.StepsToIncreaseInflation;
            }

            _autoGenericCounters = genericCounters.Where(_ => _.IsUsedInAutoStep).ToArray();
            _manualGenericCounters = genericCounters.Where(_ => !_.IsUsedInAutoStep).ToArray();
            _delayedCounters = GameCash.Counters.Values.OfType<DelayedCounter>().ToArray();
        }

        private void InitializeFundsDrivers()
        {
            Items = GetFundsDrivers();
            Items.AsParallel().ForAll(pair =>
            {
                if (pair.Value.Price == 0)
                {
                    pair.Value.Price = pair.Value.InitialPrice;
                }
                pair.Value.InflationPercent = 15;
            });
        }

        private void InitializeCustomRules()
        {
            CustomRules = GetCustomRules();
        }

        protected abstract ReadOnlyDictionary<int, Item> GetFundsDrivers();

        protected abstract GameCash GetFundsCounters();

        protected abstract ReadOnlyDictionary<int, ICustomRule> GetCustomRules();

        public async Task<int> PerformAutoStep(int iterations = 1)
        {
            if (!_isInitialized) throw new Exception("Game is not started");
            return await Task<int>.Factory.StartNew(() =>
            {
                var modifiedCounters = PerformAutoStepInternal(iterations);
                PostPerformAutoStep(modifiedCounters, iterations);
                return 0;
            });
        }

        private IEnumerable<CounterBase> PerformAutoStepInternal(int iterations = 1)
        {
            foreach (var autoGenericCounter in _autoGenericCounters)
            {
                CashFunds(autoGenericCounter.Value*iterations);
                autoGenericCounter.CurrentSteps += iterations;
                autoGenericCounter.Inflation = autoGenericCounter.CurrentSteps/
                                               autoGenericCounter.StepsToIncreaseInflation;
            }
            ;
            foreach (var manualGenericCounter in _manualGenericCounters)
            {
                manualGenericCounter.CurrentSteps -= iterations;
                manualGenericCounter.Inflation = manualGenericCounter.CurrentSteps/
                                                 manualGenericCounter.StepsToIncreaseInflation;
            }
            ;


            var delayedCounters = _delayedCounters.Where(_ => _.IsMining);
            foreach (var delayedCounter in delayedCounters)
            {
                if (delayedCounter.SecondsRemaining == 0)
                {
                    delayedCounter.IsMining = false;
                    CashFunds(delayedCounter.Value*iterations);
                    delayedCounter.SubValue *= 1.2m*iterations;
                }
                else
                {
                    delayedCounter.SecondsRemaining--;
                }
            }


            AutomaticStepNumber += iterations;
            return _autoGenericCounters;
        }

        private IEnumerable<CounterBase> PerformManualStepInternal()
        {
            foreach (var genericCounter in _manualGenericCounters)
            {
                CashFunds(genericCounter.Value);
                genericCounter.CurrentSteps++;
                genericCounter.Inflation = genericCounter.CurrentSteps/genericCounter.StepsToIncreaseInflation;
            }
            ManualStepNumber++;
            return _manualGenericCounters;
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

                var result = verficationRequiredResult != null &&
                             verficationRequiredResult.FirstNumber + verficationRequiredResult.SecondNumber ==
                             verificationData.ResultNumber
                    ? (ManualStepResult) new ManualStepVerifiedResult(true)
                    : InitializeVerificationSteps();


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
                var noVerificationResult = verificationResult as ManualStepNoVerficationRequiredResult;
                var counters = modifiedCounters as CounterBase[] ?? modifiedCounters.ToArray();
                if (noVerificationResult != null)
                {
                    var genericCounters = GameCash.Counters.Values.OfType<GenericCounter>();
                    noVerificationResult.ModifiedGameCash = new GameCash
                    {
                        Counters = genericCounters.Cast<CounterBase>().ToDictionary(_ => _.Id, counter => counter),
                        CashOnHand = GameCash.CashOnHand,
                        RootCounter = GameCash.RootCounter,
                        TotalEarned = GameCash.TotalEarned
                    };
                    verificationResult = noVerificationResult;
                }
                PostPerformManualStep(counters);
            }
            return verificationResult;
        }


        public BuyItemResult BuyFundDriver(int fundDriverId)
        {
            if (!Items.ContainsKey(fundDriverId))
            {
                throw new InvalidOperationException(string.Format("Fund driver {0} not found", fundDriverId));
            }
            var fundDriver = Items[fundDriverId];
            BuyItemResult result = null;
            if (!IsFundsDriverAvailableForBuy(fundDriver))
            {
                throw new InvalidOperationException(string.Format("Fund driver {0} not available to buy ", fundDriverId));
            }
            if (GameCash.CashOnHand >= fundDriver.Price)
            {
                PayWithFunds(fundDriver.Price);
                fundDriver.Price = fundDriver.Price +
                                          fundDriver.Price*fundDriver.InflationPercent/100.0m;
                fundDriver.Bought++;
                var changedCounters = IncrementCounters(fundDriver.Incrementors);
                PerformBuyFundDriverCustomRule(fundDriver);
                result = new BuyItemResult
                {
                    ModifiedItem = fundDriver,
                    ModifiedGameCash = new GameCash
                    {
                        Counters = changedCounters,
                        CashOnHand = GameCash.CashOnHand,
                        TotalEarned = GameCash.TotalEarned,
                        RootCounter = GameCash.RootCounter
                    }
                };
                ModifiedFundsDrivers[fundDriverId] = fundDriver;
            }
            PostBuyFundDriver(fundDriver);
            return result;
        }

        private void PerformBuyFundDriverCustomRule(Item fundDriver)
        {
            if (fundDriver.CustomRuleInfo != null && CustomRules.ContainsKey(fundDriver.CustomRuleInfo.CustomRule.Id))
            {
                CustomRules[fundDriver.CustomRuleInfo.CustomRule.Id].PerformRuleWhenBuyFundDriver(this,
                    fundDriver.CustomRuleInfo);
            }
        }

        protected virtual void PostPerformManualStep(IEnumerable<CounterBase> modifiedCounters)
        {
        }

        protected virtual void PostPerformAutoStep(IEnumerable<CounterBase> modifiedCounters, int iterations)
        {
        }

        protected virtual void PostBuyFundDriver(Item fundDriver)
        {
        }

        protected virtual void PostInitialize()
        {
        }

        protected virtual void CashFunds(decimal value)
        {
            GameCash.CashOnHand += value;
            GameCash.TotalEarned += value;
        }

        protected virtual void PayWithFunds(decimal value)
        {
            GameCash.CashOnHand -= value;
        }

        protected IDictionary<int, CounterBase> IncrementCounters(IDictionary<int, IncrementorBase> incrementors)
        {
            IDictionary<int, CounterBase> result = new Dictionary<int, CounterBase>();
            foreach (var counter in GameCash.Counters)
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
                    result.Add(counter);
                }
            }
            return result;
        }

        protected string GetIncrementorValueById(Item item, int incrmentorId)
        {
            var incrementor = item.Incrementors.ContainsKey(incrmentorId)
                ? item.Incrementors[incrmentorId]
                : null;
            if (incrementor != null)
            {
                return string.Format("{0}{1}", incrementor.Value,
                    incrementor.IncrementorType == IncrementorTypeEnum.PercentageIncrementor ? "%" : string.Empty);
            }
            return "0";
        }

        protected bool IsFundsDriverAvailableForBuy(Item item)
        {
            return item.UnlockBalance <= GameCash.RootCounter.Value;
        }


        public void FightAgainstInflation()
        {
            if (_autoGenericCounters.All(_ => _.Inflation == 0))
            {
                return;
            }

            foreach (var genericCounter in _autoGenericCounters)
            {
                genericCounter.CurrentSteps -= genericCounter.StepsToIncreaseInflation;
                genericCounter.Inflation = genericCounter.CurrentSteps/genericCounter.StepsToIncreaseInflation;
                CashFunds(genericCounter.Value);
            }
            GameCash.RootCounter.SubValue += 1;
        }

        public void ActivateDelayedCounter(int counterId)
        {
            var delayedCounter = (DelayedCounter) GameCash.Counters[counterId];
            if (delayedCounter == null)
            {
                return;
            }
            if (delayedCounter.IsMining | delayedCounter.UnlockValue > GameCash.RootCounter.Value)
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

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}