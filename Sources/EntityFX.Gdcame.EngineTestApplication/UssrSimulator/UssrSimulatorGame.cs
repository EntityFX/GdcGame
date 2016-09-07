﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.Gdcame.GameEngine;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.GameEngine.CustomRules;

namespace EntityFX.Gdcame.EngineTestApplication.UssrSimulator
{
    public class UssrSimulatorGame : GameBase
    {
        private readonly object _lockObject = new {};

        public decimal Communism
        {
            get { return GameCash.Counters[(int) UssrCounterEnum.Communism].SubValue; }
        }

        public decimal Production
        {
            get { return GameCash.Counters[(int) UssrCounterEnum.Production].SubValue; }
        }

        public decimal Taxes
        {
            get { return GameCash.Counters[(int) UssrCounterEnum.Tax].SubValue; }
        }

        public decimal FiveYearPlan
        {
            get { return GameCash.Counters[(int) UssrCounterEnum.FiveYearPlan].SubValue; }
        }

        protected override ReadOnlyDictionary<int, Item> GetFundsDrivers()
        {
            return new ReadOnlyDictionary<int, Item>(new Dictionary<int, Item>
            {
                {
                    1,
                    new Item
                    {
                        InitialPrice = 200,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(10)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(1)
                            }
                        },
                        Name = "Matches"
                    }
                },
                {
                    2,
                    new Item
                    {
                        InitialPrice = 400,
                        UnlockBalance = 5,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(10)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(1)
                            }
                        },
                        Name = "Bubble gum"
                    }
                },
                {
                    3,
                    new Item
                    {
                        InitialPrice = 800,
                        UnlockBalance = 10,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(20)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(20)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(2)
                            }
                        },
                        Name = "Beer",
                        CustomRuleInfo = new CustomRuleInfo
                        {
                            CustomRule = CustomRules[1]
                        }
                    }
                },
                {
                    4,
                    new Item
                    {
                        InitialPrice = 1500,
                        UnlockBalance = 25,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(80)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(40)
                            }
                        },
                        Name = "Voice Of America",
                        CustomRuleInfo = new CustomRuleInfo
                        {
                            CustomRule = CustomRules[2]
                        }
                    }
                },
                {
                    5,
                    new Item
                    {
                        InitialPrice = 3000,
                        UnlockBalance = 40,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(160)
                            }
                        },
                        Name = "Dissidence On Flat",
                        CustomRuleInfo = new CustomRuleInfo
                        {
                            CustomRule = CustomRules[3]
                        }
                    }
                },
                {
                    6,
                    new Item
                    {
                        InitialPrice = 6000,
                        UnlockBalance = 80,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            }
                        },
                        Name = "Hucksterism"
                    }
                },
                {
                    7,
                    new Item
                    {
                        InitialPrice = 10000,
                        UnlockBalance = 120,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(240)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            }
                        },
                        Name = "Soda Machine"
                    }
                },
                {
                    8,
                    new Item
                    {
                        InitialPrice = 20000,
                        UnlockBalance = 180,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(80)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            }
                        },
                        Name = "Grocery Store"
                    }
                },
                {
                    9,
                    new Item
                    {
                        InitialPrice = 50000,
                        UnlockBalance = 220,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new PercentageIncrementor(2)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            }
                        },
                        Name = "Black market"
                    }
                },
                {
                    10,
                    new Item
                    {
                        InitialPrice = 223200,
                        UnlockBalance = 250,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(150)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            }
                        },
                        Name = "Poster agitation"
                    }
                },
                {
                    11,
                    new Item
                    {
                        InitialPrice = 250000,
                        UnlockBalance = 280,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(100)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(150)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            }
                        },
                        Name = "TV Shows"
                    }
                },
                {
                    12,
                    new Item
                    {
                        InitialPrice = 300000,
                        UnlockBalance = 500,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(200)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            }
                        },
                        Name = "Censore"
                    }
                },
                {
                    13,
                    new Item
                    {
                        InitialPrice = 500000,
                        UnlockBalance = 600,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(250)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(5)
                            }
                        },
                        Name = "Excursion to the mausoleum"
                    }
                },
                {
                    14,
                    new Item
                    {
                        InitialPrice = 800000,
                        UnlockBalance = 650,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(30)
                            }
                        },
                        Name = "Build Lenin Statue"
                    }
                },
                {
                    15,
                    new Item
                    {
                        InitialPrice = 1100000,
                        UnlockBalance = 700,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new PercentageIncrementor(5)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            }
                        },
                        Name = "Electrofication"
                    }
                },
                {
                    16,
                    new Item
                    {
                        InitialPrice = 1100000,
                        UnlockBalance = 800,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(100)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(100)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            }
                        },
                        Name = "Canned Production"
                    }
                },
                {
                    17,
                    new Item
                    {
                        InitialPrice = 1350000,
                        UnlockBalance = 900,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(150)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(200)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            }
                        },
                        Name = "Candies Production"
                    }
                },
                {
                    18,
                    new Item
                    {
                        InitialPrice = 2200000,
                        UnlockBalance = 1000,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(300)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(50)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(15)
                            }
                        },
                        Name = "Champagne Production"
                    }
                },
                {
                    19,
                    new Item
                    {
                        InitialPrice = 2500000,
                        UnlockBalance = 1200,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Tax,
                                new PercentageIncrementor(2)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            }
                        },
                        Name = "Collectivisation"
                    }
                },
                {
                    20,
                    new Item
                    {
                        InitialPrice = 2500000,
                        UnlockBalance = 1300,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(500)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(20)
                            }
                        },
                        Name = "Corn fields"
                    }
                },
                {
                    21,
                    new Item
                    {
                        InitialPrice = 1000000,
                        UnlockBalance = 1500,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(500)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(25)
                            }
                        },
                        Name = "Virign Land Campaign"
                    }
                },
                {
                    22,
                    new Item
                    {
                        InitialPrice = 2500000,
                        UnlockBalance = 1800,
                        Incrementors = new Dictionary<int, IncrementorBase>
                        {
                            {
                                (int) UssrCounterEnum.Production,
                                new ValueIncrementor(500)
                            },
                            {
                                (int) UssrCounterEnum.Tax,
                                new ValueIncrementor(150)
                            },
                            {
                                (int) UssrCounterEnum.Communism,
                                new ValueIncrementor(15)
                            }
                        },
                        Name = "Cinema"
                    }
                }
            });
        }

        protected override GameCash GetFundsCounters()
        {
            var counters = new Dictionary<int, CounterBase>
            {
                {
                    (int) UssrCounterEnum.Communism,
                    new SingleCounter
                    {
                        Id = 0,
                        Name = "Communism"
                    }
                },
                {
                    (int) UssrCounterEnum.Production,
                    new GenericCounter
                    {
                        Id = 1,
                        Name = "Production",
                        SubValue = 10,
                        StepsToIncreaseInflation = 1000
                    }
                },
                {
                    (int) UssrCounterEnum.Tax,
                    new GenericCounter
                    {
                        Id = 2,
                        Name = "Tax",
                        IsUsedInAutoStep = true,
                        StepsToIncreaseInflation = 2000
                    }
                },
                {
                    (int) UssrCounterEnum.FiveYearPlan,
                    new DelayedCounter
                    {
                        Id = 3,
                        Name = "Five Year Plan",
                        UnlockValue = 10000,
                        SecondsToAchieve = 14400,
                        SecondsRemaining = 0,
                        IsMining = false,
                        SubValue = 5000000
                    }
                }
            };

            return new GameCash
            {
                Counters = counters,
                RootCounter = counters[(int) UssrCounterEnum.Communism]
            };
        }

        protected override ReadOnlyDictionary<int, ICustomRule> GetCustomRules()
        {
            return new ReadOnlyDictionary<int, ICustomRule>(new Dictionary<int, ICustomRule>
            {
                {1, new DelayedCounterCustomRule {Id = 1}},
                {2, new ReduceFundDriverPriceCustomRule {Id = 2}},
                {3, new IncreaseFundDriverIncrementorsCustomRule {Id = 3}}
            });
        }

        protected override void PostPerformManualStep(IEnumerable<CounterBase> modifiedCounters)
        {
        }

        protected override void PostPerformAutoStep(IEnumerable<CounterBase> modifiedCounters, int iterations)
        {
        }

        protected override void PostInitialize()
        {
        }

        protected override void PostBuyFundDriver(Item fundDriverId)
        {
        }
    }
}