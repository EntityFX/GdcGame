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

        public Item[] Items { get; private set; }

        public Dictionary<int, Item> ModifiedFundsDrivers { get; private set; }

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

        private static readonly object _lockState = new { };
        private GenericCounter[] _genericCounters;
        private GenericCounter[] _autoGenericCounters;
        private GenericCounter[] _manualGenericCounters;
        private DelayedCounter[] _delayedCounters;

        protected GameBase()
        {
            ModifiedFundsDrivers = new Dictionary<int, Item>();
        }

        public void Initialize()
        {
            PreInitialize();
            InitializeCounters();
            InitializeCustomRules();
            InitializeItems();
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
            var delta = (int)(_stepsToVerify * 0.1);
            _nextVerificationSteps = _stepsToVerify + _randomiserForVerification.Next(-1 * delta, delta);
            return verificationManualStepResult;
        }

        protected virtual void PreInitialize()
        {
        }

        private void InitializeCounters()
        {
            GameCash = GetFundsCounters();
            _genericCounters = GameCash.Counters.OfType<GenericCounter>().ToArray();
            foreach (var fundCounter in _genericCounters)
            {
                fundCounter.StepsToIncreaseInflation = fundCounter.StepsToIncreaseInflation == 0
                    ? 1000
                    : fundCounter.StepsToIncreaseInflation;
            }

            _autoGenericCounters = _genericCounters.Where(_ => _.IsUsedInAutoStep).ToArray();
            _manualGenericCounters = _genericCounters.Where(_ => !_.IsUsedInAutoStep).ToArray();
            _delayedCounters = GameCash.Counters.OfType<DelayedCounter>().ToArray();
        }

        private void InitializeItems()
        {
            Items = GetFundsDrivers();

            foreach (var item in Items)
            {
                if (item.Price == 0)
                {
                    item.Price = item.InitialPrice;
                }
                item.InflationPercent = 15;
            };
        }

        private void InitializeCustomRules()
        {
            CustomRules = GetCustomRules();
        }

        protected abstract Item[] GetFundsDrivers();

        protected abstract GameCash GetFundsCounters();

        protected abstract ReadOnlyDictionary<int, ICustomRule> GetCustomRules();

        public void PerformAutoStep(int iterations = 1)
        {
            if (!_isInitialized) throw new Exception("Game is not started");
            PostPerformAutoStep(PerformAutoStepInternal(iterations), iterations);

        }

        private CounterBase[] PerformAutoStepInternal(int iterations = 1)
        {
            int i = 0;
            GenericCounter counter;
            DelayedCounter delayedCounter;
            bool usedInAutoStep;
            for (i = 0; i < _genericCounters.Length; i++)
            {
                counter = _genericCounters[i];
                usedInAutoStep = counter.IsUsedInAutoStep;
                if (usedInAutoStep)
                {
                    AddCash(counter.Value * iterations);
                }
                counter.CurrentSteps = counter.CurrentSteps + (usedInAutoStep ? iterations : -iterations);
                counter.Inflation = counter.CurrentSteps /
                                               counter.StepsToIncreaseInflation;
            }
            for (i = 0; i < _delayedCounters.Length; i++)
            {
                delayedCounter = _delayedCounters[i];
                if (!delayedCounter.IsMining)
                {
                    continue;
                }
                if (delayedCounter.SecondsRemaining == 0)
                {
                    delayedCounter.IsMining = false;
                    AddCash(delayedCounter.Value * iterations);
                    delayedCounter.SubValue *= 1.2m * iterations;
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
            GenericCounter counter;
            for (var i = 0; i < _manualGenericCounters.Length; i++)
            {
                counter = _manualGenericCounters[i];
                AddCash(counter.Value);
                counter.CurrentSteps++;
                counter.Inflation = counter.CurrentSteps / counter.StepsToIncreaseInflation;
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
                    ? (ManualStepResult)new ManualStepVerifiedResult(true)
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
                    var genericCounters = GameCash.Counters.OfType<GenericCounter>();
                    noVerificationResult.ModifiedGameCash = new GameCash
                    {
                        Counters = genericCounters.Cast<CounterBase>().ToArray(),
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
            if (Items.Length < fundDriverId)
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
                                          fundDriver.Price * fundDriver.InflationPercent / 100.0m;
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

        protected virtual void PostPerformAutoStep(CounterBase[] modifiedCounters, int iterations)
        {
        }

        protected virtual void PostBuyFundDriver(Item fundDriver)
        {
        }

        protected virtual void PostInitialize()
        {
        }

        protected virtual void AddCash(decimal value)
        {
            GameCash.CashOnHand += value;
            GameCash.TotalEarned += value;
        }

        protected virtual void PayWithFunds(decimal value)
        {
            GameCash.CashOnHand -= value;
        }

        protected CounterBase[] IncrementCounters(IncrementorBase[] incrementors)
        {
            foreach (var counter in GameCash.Counters)
            {
                if (counter.Id >= incrementors.Length)
                {
                    break;
                }
                IncrementorBase incrementor = incrementors[counter.Id];
                if (incrementor.IncrementorType == IncrementorTypeEnum.ValueIncrementor)
                {
                    var genericCounter = counter as GenericCounter;
                    if (genericCounter != null)
                    {
                        genericCounter.SubValue += incrementor.Value;
                    }
                    else
                    {
                        counter.SubValue += incrementors[counter.Id].Value;
                    }
                }
                else if (incrementor.IncrementorType == IncrementorTypeEnum.PercentageIncrementor)
                {
                    var genericCounter = counter as GenericCounter;
                    if (genericCounter != null)
                    {
                        genericCounter.BonusPercentage += incrementor.Value;
                    }
                }
            }
            return GameCash.Counters;
        }

        protected string GetIncrementorValueById(Item item, int incrmentorId)
        {
            var incrementor = item.Incrementors.Length > incrmentorId
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
                genericCounter.Inflation = genericCounter.CurrentSteps / genericCounter.StepsToIncreaseInflation;
                AddCash(genericCounter.Value);
            }
            GameCash.RootCounter.SubValue += 1;
        }

        public void ActivateDelayedCounter(int counterId)
        {
            var delayedCounter = (DelayedCounter)GameCash.Counters[counterId];
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