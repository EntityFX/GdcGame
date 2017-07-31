namespace EntityFX.Gdcame.Kernel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Items;

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
            this.ModifiedFundsDrivers = new Dictionary<int, Item>();
        }

        public void Initialize()
        {
            this.PreInitialize();
            this.InitializeCounters();
            this.InitializeCustomRules();
            this.InitializeItems();
            this.InitializeVerificationSteps();
            this.PostInitialize();
            this._isInitialized = true;
        }

        private ManualStepVerificationRequiredResult InitializeVerificationSteps()
        {
            var verificationManualStepResult = new ManualStepVerificationRequiredResult
            {
                FirstNumber = this._randomiserForVerification.Next(1, 50),
                SecondNumber = this._randomiserForVerification.Next(1, 50)
            };
            this._currentVerificationSteps = 0;
            var delta = (int)(this._stepsToVerify * 0.1);
            this._nextVerificationSteps = this._stepsToVerify + this._randomiserForVerification.Next(-1 * delta, delta);
            return verificationManualStepResult;
        }

        protected virtual void PreInitialize()
        {
        }

        private void InitializeCounters()
        {
            this.GameCash = this.GetFundsCounters();
            this._genericCounters = this.GameCash.Counters.OfType<GenericCounter>().ToArray();
            foreach (var fundCounter in this._genericCounters)
            {
                fundCounter.StepsToIncreaseInflation = fundCounter.StepsToIncreaseInflation == 0
                    ? 1000
                    : fundCounter.StepsToIncreaseInflation;
            }

            this._autoGenericCounters = this._genericCounters.Where(_ => _.IsUsedInAutoStep).ToArray();
            this._manualGenericCounters = this._genericCounters.Where(_ => !_.IsUsedInAutoStep).ToArray();
            this._delayedCounters = this.GameCash.Counters.OfType<DelayedCounter>().ToArray();
        }

        private void InitializeItems()
        {
            this.Items = this.GetFundsDrivers();

            foreach (var item in this.Items)
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
            this.CustomRules = this.GetCustomRules();
        }

        protected abstract Item[] GetFundsDrivers();

        protected abstract GameCash GetFundsCounters();

        protected abstract ReadOnlyDictionary<int, ICustomRule> GetCustomRules();

        public void PerformAutoStep(int iterations = 1)
        {
            if (!this._isInitialized) throw new Exception("Game is not started");
            this.PostPerformAutoStep(this.PerformAutoStepInternal(iterations), iterations);

        }

        private CounterBase[] PerformAutoStepInternal(int iterations = 1)
        {
            int i = 0;
            GenericCounter counter;
            DelayedCounter delayedCounter;
            bool usedInAutoStep;
            for (i = 0; i < this._genericCounters.Length; i++)
            {
                counter = this._genericCounters[i];
                usedInAutoStep = counter.IsUsedInAutoStep;
                if (usedInAutoStep)
                {
                    this.AddCash(counter.Value * iterations);
                }
                counter.CurrentSteps = counter.CurrentSteps + (usedInAutoStep ? iterations : -iterations);
                counter.Inflation = counter.CurrentSteps /
                                               counter.StepsToIncreaseInflation;
            }
            for (i = 0; i < this._delayedCounters.Length; i++)
            {
                delayedCounter = this._delayedCounters[i];
                if (!delayedCounter.IsMining)
                {
                    continue;
                }
                if (delayedCounter.SecondsRemaining == 0)
                {
                    delayedCounter.IsMining = false;
                    this.AddCash(delayedCounter.Value * iterations);
                    delayedCounter.SubValue *= 1.2m * iterations;
                }
                else
                {
                    delayedCounter.SecondsRemaining--;
                }
            }

            this.AutomaticStepNumber += iterations;
            return this._autoGenericCounters;
        }

        private IEnumerable<CounterBase> PerformManualStepInternal()
        {
            GenericCounter counter;
            for (var i = 0; i < this._manualGenericCounters.Length; i++)
            {
                counter = this._manualGenericCounters[i];
                this.AddCash(counter.Value);
                counter.CurrentSteps++;
                counter.Inflation = counter.CurrentSteps / counter.StepsToIncreaseInflation;
            }
            this.ManualStepNumber++;
            return this._manualGenericCounters;
        }

        private ManualStepResult VerifyManualStep(VerificationManualStepData verificationData)
        {
            if (this._manualStepVerificationRequiredResult != null)
            {
                if (verificationData == null)
                {
                    return this._manualStepVerificationRequiredResult;
                }
                var verficationRequiredResult =
                    this._manualStepVerificationRequiredResult as ManualStepVerificationRequiredResult;

                var result = verficationRequiredResult != null &&
                             verficationRequiredResult.FirstNumber + verficationRequiredResult.SecondNumber ==
                             verificationData.ResultNumber
                    ? (ManualStepResult)new ManualStepVerifiedResult(true)
                    : this.InitializeVerificationSteps();


                if (!result.IsVerificationRequired)
                {
                    this._manualStepVerificationRequiredResult = null;
                }
                else
                {
                    this._manualStepVerificationRequiredResult = result;
                }
                return result;
            }

            if (this._currentVerificationSteps < this._nextVerificationSteps)
            {
                this._currentVerificationSteps++;
            }
            else
            {
                this._manualStepVerificationRequiredResult = this.InitializeVerificationSteps();
                return this._manualStepVerificationRequiredResult;
            }

            return new ManualStepNoVerficationRequiredResult();
        }

        public ManualStepResult PerformManualStep(VerificationManualStepData verificationData)
        {
            if (!this._isInitialized) throw new InvalidOperationException("Game is not started");

            var verificationResult = this.VerifyManualStep(verificationData);
            IEnumerable<CounterBase> modifiedCounters = null;

            if (!verificationResult.IsVerificationRequired)
            {
                modifiedCounters = this.PerformManualStepInternal();
                var noVerificationResult = verificationResult as ManualStepNoVerficationRequiredResult;
                var counters = modifiedCounters as CounterBase[] ?? modifiedCounters.ToArray();
                if (noVerificationResult != null)
                {
                    var genericCounters = this.GameCash.Counters.OfType<GenericCounter>();
                    noVerificationResult.ModifiedGameCash = new GameCash
                    {
                        Counters = genericCounters.Cast<CounterBase>().ToArray(),
                        CashOnHand = this.GameCash.CashOnHand,
                        RootCounter = this.GameCash.RootCounter,
                        TotalEarned = this.GameCash.TotalEarned
                    };
                    verificationResult = noVerificationResult;
                }
                this.PostPerformManualStep(counters);
            }
            return verificationResult;
        }


        public BuyItemResult BuyFundDriver(int fundDriverId)
        {
            if (this.Items.Length < fundDriverId)
            {
                throw new InvalidOperationException(string.Format("Fund driver {0} not found", fundDriverId));
            }
            var fundDriver = this.Items[fundDriverId];
            BuyItemResult result = null;
            if (!this.IsFundsDriverAvailableForBuy(fundDriver))
            {
                throw new InvalidOperationException(string.Format("Fund driver {0} not available to buy ", fundDriverId));
            }
            if (this.GameCash.CashOnHand >= fundDriver.Price)
            {
                this.PayWithFunds(fundDriver.Price);
                fundDriver.Price = fundDriver.Price +
                                          fundDriver.Price * fundDriver.InflationPercent / 100.0m;
                fundDriver.Bought++;
                var changedCounters = this.IncrementCounters(fundDriver.Incrementors);
                this.PerformBuyFundDriverCustomRule(fundDriver);
                result = new BuyItemResult
                {
                    ModifiedItem = fundDriver,
                    ModifiedGameCash = new GameCash
                    {
                        Counters = changedCounters,
                        CashOnHand = this.GameCash.CashOnHand,
                        TotalEarned = this.GameCash.TotalEarned,
                        RootCounter = this.GameCash.RootCounter
                    }
                };
                this.ModifiedFundsDrivers[fundDriverId] = fundDriver;
            }
            this.PostBuyFundDriver(fundDriver);
            return result;
        }

        private void PerformBuyFundDriverCustomRule(Item fundDriver)
        {
            if (fundDriver.CustomRuleInfo != null && this.CustomRules.ContainsKey(fundDriver.CustomRuleInfo.CustomRule.Id))
            {
                this.CustomRules[fundDriver.CustomRuleInfo.CustomRule.Id].PerformRuleWhenBuyFundDriver(this,
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
            this.GameCash.CashOnHand += value;
            this.GameCash.TotalEarned += value;
        }

        protected virtual void PayWithFunds(decimal value)
        {
            this.GameCash.CashOnHand -= value;
        }

        protected CounterBase[] IncrementCounters(IncrementorBase[] incrementors)
        {
            foreach (var counter in this.GameCash.Counters)
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
            return this.GameCash.Counters;
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
            return item.UnlockBalance <= this.GameCash.RootCounter.Value;
        }


        public void FightAgainstInflation()
        {
            if (this._autoGenericCounters.All(_ => _.Inflation == 0))
            {
                return;
            }

            foreach (var genericCounter in this._autoGenericCounters)
            {
                genericCounter.CurrentSteps -= genericCounter.StepsToIncreaseInflation;
                genericCounter.Inflation = genericCounter.CurrentSteps / genericCounter.StepsToIncreaseInflation;
                this.AddCash(genericCounter.Value);
            }
            this.GameCash.RootCounter.SubValue += 1;
        }

        public void ActivateDelayedCounter(int counterId)
        {
            var delayedCounter = (DelayedCounter)this.GameCash.Counters[counterId];
            if (delayedCounter == null)
            {
                return;
            }
            if (delayedCounter.IsMining | delayedCounter.UnlockValue > this.GameCash.RootCounter.Value)
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