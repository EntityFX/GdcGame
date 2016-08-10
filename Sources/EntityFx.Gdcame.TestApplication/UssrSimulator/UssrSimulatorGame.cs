using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.Gdcame.GameEngine;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.CustomRules;

namespace EntityFx.Gdcame.TestApplication.UssrSimulator
{
    public class UssrSimulatorGame : GameBase
    {
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
            return new Dictionary<int, FundsDriver> {
                {
                    1,
                    new FundsDriver {
                        InitialValue = 200,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(10)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(1)
                            } ,
                        },
                        Name = "Matches"
                    }   
                },
                {
                    2,
                    new FundsDriver {
                        InitialValue = 400,
                        UnlockValue = 5,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(10)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(1)
                            } ,
                        },
                        Name = "Bubble gum"
                    }   
                },
                {
                    3,
                    new FundsDriver {
                        InitialValue = 800,
                        UnlockValue = 10,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(20)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(20)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(2)
                            } ,
                        },
                        Name = "Beer",
                        CustomRuleInfo = new CustomRuleInfo()
                        {
                            CustomRule = CustomRules[1]
                        }
                    }   
                },
                {
                    4,
                    new FundsDriver {
                        InitialValue = 1500,
                        UnlockValue = 25,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(80)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(40)
                            },
                        },
                        Name = "Voice Of America",
                        CustomRuleInfo = new CustomRuleInfo()
                        {
                            CustomRule = CustomRules[2]
                        }
                    }   
                },
                {
                    5,
                    new FundsDriver {
                        InitialValue = 3000,
                        UnlockValue = 40,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(160)
                            } ,
                        },
                        Name = "Dissidence On Flat",
                        CustomRuleInfo = new CustomRuleInfo()
                        {
                            CustomRule = CustomRules[3]
                        }
                    }   
                },
                {
                    6,
                    new FundsDriver {
                        InitialValue = 6000,
                        UnlockValue = 80,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            } ,
                        },
                        Name = "Hucksterism"
                    }   
                },
                {
                    7,
                    new FundsDriver {
                        InitialValue = 10000,
                        UnlockValue = 120,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(240)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            } ,
                        },
                        Name = "Soda Machine"
                    }   
                },
                {
                    8,
                    new FundsDriver {
                        InitialValue = 20000,
                        UnlockValue = 180,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(80)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            } ,
                        },
                        Name = "Grocery Store"
                    }   
                },
                {
                    9,
                    new FundsDriver {
                        InitialValue = 50000,
                        UnlockValue = 220,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new PercentageIncrementor(2)
                            },
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Black market"
                    }   
                },
                {
                    10,
                    new FundsDriver {
                        InitialValue = 223200,
                        UnlockValue = 250,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(150)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Poster agitation"
                    }   
                },
                {
                    11,
                    new FundsDriver {
                        InitialValue = 250000,
                        UnlockValue = 280,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(100)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(150)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            } ,
                        },
                        Name = "TV Shows"
                    }   
                },
                {
                    12,
                    new FundsDriver {
                        InitialValue = 300000,
                        UnlockValue = 500,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(200)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Censore"
                    }   
                },
                {
                    13,
                    new FundsDriver {
                        InitialValue = 500000,
                        UnlockValue = 600,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(250)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            },
                        },
                        Name = "Excursion to the mausoleum"
                    }   
                },
                {
                    14,
                    new FundsDriver {
                        InitialValue = 800000,
                        UnlockValue = 650,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(30)
                            } ,
                        },
                        Name = "Build Lenin Statue"
                    }   
                },
                {
                    15,
                    new FundsDriver {
                        InitialValue = 1100000,
                        UnlockValue = 700,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new PercentageIncrementor(5)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Electrofication"
                    }   
                },
                {
                    16,
                    new FundsDriver {
                        InitialValue = 1100000,
                        UnlockValue = 800,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(100)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(100)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Canned Production"
                    }   
                },
                {
                    17,
                    new FundsDriver {
                        InitialValue = 1350000,
                        UnlockValue = 900,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(150)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(200)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Candies Production"
                    }   
                },
                {
                    18,
                    new FundsDriver {
                        InitialValue = 2200000,
                        UnlockValue = 1000,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(300)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(50)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(15)
                            } ,
                        },
                        Name = "Champagne Production"
                    }   
                },
                {
                    19,
                    new FundsDriver {
                        InitialValue = 2500000,
                        UnlockValue = 1200,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Tax,
                                new PercentageIncrementor(2)
                            },
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Collectivisation"
                    }   
                },
                {
                    20,
                    new FundsDriver {
                        InitialValue = 2500000,
                        UnlockValue = 1300,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(500)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(20)
                            } ,
                        },
                        Name = "Corn fields"
                    }   
                },
                {
                    21,
                    new FundsDriver {
                        InitialValue = 1000000,
                        UnlockValue = 1500,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(500)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(25)
                            } ,
                        },
                        Name = "Virign Land Campaign"
                    }   
                },
                                {
                    22,
                    new FundsDriver {
                        InitialValue = 2500000,
                        UnlockValue = 1800,
                        Incrementors = new Dictionary<int, IncrementorBase> {
                                                        {
                                (int)UssrCounterEnum.Production,
                                new ValueIncrementor(500)
                            } ,
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(150)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(15)
                            } ,
                        },
                        Name = "Cinema"
                    }   
                },
            };
        }

        protected override FundsCounters GetFundsCounters()
        {
            var counters = new Dictionary<int, CounterBase>() { 
                    {
                        (int)UssrCounterEnum.Communism,
                        new SingleCounter {
                            Id = 0,
                            Name = "Communism"
                        } 
                    },
                    {
                        (int)UssrCounterEnum.Production,  
                        new GenericCounter {
                            Id = 1,
                            Name = "Production",
                            SubValue = 10,
                            StepsToIncreaseInflation = 1000
                        } 
                    }, 
                    { 
                        (int)UssrCounterEnum.Tax, 
                        new GenericCounter {
                            Id = 2,
                            Name = "Tax",
                            IsUsedInAutoStep = true,
                            StepsToIncreaseInflation = 2000
                        }   
                    },
                    { 
                        (int)UssrCounterEnum.FiveYearPlan, 
                        new DelayedCounter {
                            Id = 3,
                            Name = "Five Year Plan",
                            UnlockValue = 10000,
                            SecondsToAchieve = 14400,
                            SecondsRemaining = 0,
                            IsMining = false,
                            SubValue = 5000000
                        }   
                    },
                };

            return new FundsCounters()
            {
                Counters = counters,
                RootCounter = counters[(int)UssrCounterEnum.Communism]
            };
        }

        protected override IDictionary<int, ICustomRule> GetCustomRules()
        {
            return new ReadOnlyDictionary<int, ICustomRule>(new Dictionary<int, ICustomRule>()
            {
                {1, new DelayedCounterCustomRule() { Id = 1}},
                {2, new ReduceFundDriverPriceCustomRule() { Id = 2}},
                {3, new IncreaseFundDriverIncrementorsCustomRule() { Id = 3}},
            });
        }

        private readonly object _lockObject = new { };

        private int verifyResult = -1;

        protected override void PostPerformManualStep(IEnumerable<CounterBase> modifiedCounters)
        {

        }

        protected override void PostPerformAutoStep(IEnumerable<CounterBase> modifiedCounters, int iterations)
        {


        }

        protected override void PostInitialize()
        {

        }

        protected override void PostBuyFundDriver(FundsDriver fundDriverId)
        {

        }


    }
}
