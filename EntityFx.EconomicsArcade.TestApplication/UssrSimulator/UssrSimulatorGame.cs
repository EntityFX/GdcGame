using System;
using System.Collections.Generic;
using EntityFX.EconomicArcade.Engine.GameEngine;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;

namespace EntityFx.EconomicsArcade.TestApplication.UssrSimulator
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
                        Name = "Beer"
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
                        Name = "Voice Of America"
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
                        Name = "Dissidence On Flat"
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
                            Name = "Communism"
                        } 
                    },
                    {
                        (int)UssrCounterEnum.Production,  
                        new GenericCounter {
                            Name = "Production",
                            SubValue = 10,
                            StepsToIncreaseInflation = 1000
                        } 
                    }, 
                    { 
                        (int)UssrCounterEnum.Tax, 
                        new GenericCounter {
                            Name = "Tax",
                            IsUsedInAutoStep = true,
                            StepsToIncreaseInflation = 2000
                        }   
                    },
                    { 
                        (int)UssrCounterEnum.FiveYearPlan, 
                        new DelayedCounter {
                            Name = "Five Year Plan",
                            UnlockValue = 10000,
                            SecondsToAchieve = 10,
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

        private readonly object _lockObject = new { };

        protected override void PostPerformManualStep(IEnumerable<CounterBase> modifiedCounters)
        {
            DisplayGameData();
        }

        protected override void PostPerformAutoStep(IEnumerable<CounterBase> modifiedCounters)
        {

            DisplayGameData();

        }

        protected override void PostInitialize()
        {
            //CashFunds(1500000);
            Console.CursorVisible = false;
            DisplayGameData();
        }

        protected override void PostBuyFundDriver(FundsDriver fundDriverId)
        {
            DisplayGameData();
        }

        private void DisplayGameData()
        {
            lock (_lockObject)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Funds: {0:C}; Total Funds: {1:C}", FundsCounters.CurrentFunds, FundsCounters.TotalFunds);
                Console.WriteLine("Manual Steps: {0}, Automatic Steps: {1}", 
                    ManualStepNumber, AutomaticStepNumber);
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "{1,15}: {0,12}", Communism, FundsCounters.Counters[(int)UssrCounterEnum.Communism].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ", Production, FundsCounters.Counters[(int)UssrCounterEnum.Production].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Production]).Bonus, "Bonus"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Production]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{1,15}: {0,12}%"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Production]).Inflation, "Inflation");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}", ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Production]).StepsToIncreaseInflation, ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Production]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Production]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}", Taxes, FundsCounters.Counters[(int)UssrCounterEnum.Tax].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Tax]).Bonus, "Bonus"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Tax]).BonusPercentage);
                PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Tax]).Inflation, "Corruption");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGray, " StepsToIncrInflation: {0}, Current Steps: {1}", ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Tax]).StepsToIncreaseInflation, ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Tax]).CurrentSteps);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                    , ((GenericCounter)FundsCounters.Counters[(int)UssrCounterEnum.Tax]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: +{0,12:C} {2}", FiveYearPlan, FundsCounters.Counters[(int)UssrCounterEnum.FiveYearPlan].Name, TimeSpan.FromSeconds(((DelayedCounter)FundsCounters.Counters[(int)UssrCounterEnum.FiveYearPlan]).SecondsRemaining));
                
                Console.WriteLine();
                int charIndex = 65;
                PrettyConsole.WriteLineColor(ConsoleColor.DarkYellow, "{0,2}:             Fight Against Corruption", "*");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkYellow, "{0,2}:                    Do Five Year Plan", "+");
                foreach (var fundsDriver in FundsDrivers)
                {
                    if (!IsFundsDriverAvailableForBuy(fundsDriver.Value))
                    {
                        PrettyConsole.WriteColor(ConsoleColor.DarkGray, "{0,2}:             Need money to buy:     {1,8}. x{2,-4} ", ((char)charIndex).ToString(), fundsDriver.Value.UnlockValue, fundsDriver.Value.BuyCount);
                    }
                    else
                    {
                        PrettyConsole.WriteColor(ConsoleColor.White, "{3,2}: {0,28} {1,15:C} x{2,-4} ", fundsDriver.Value.Name, fundsDriver.Value.CurrentValue, fundsDriver.Value.BuyCount, ((char)charIndex).ToString());
                    }
                    PrettyConsole.WriteColor(ConsoleColor.Red, "+{0, -4} ", GetIncrementorValueById(fundsDriver.Value, (int)UssrCounterEnum.Communism));
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "+{0, -7} ", GetIncrementorValueById(fundsDriver.Value, (int)UssrCounterEnum.Production));
                    PrettyConsole.WriteColor(ConsoleColor.Green, "+{0,-7} ", GetIncrementorValueById(fundsDriver.Value, (int)UssrCounterEnum.Tax));
                    Console.WriteLine();
                    charIndex++;
                }
            }
        }
    }

    internal static class PrettyConsole
    {
        private static readonly object LockObject = new { };

        public static void WriteColor(ConsoleColor color, string text)
        {
            WriteColor(color, text, null);
        }

        public static void WriteColor(ConsoleColor color, string text, params object[] args)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text, args);
            Console.ForegroundColor = fg;
        }

        public static void WriteLineColor(ConsoleColor color, string text)
        {
            WriteLineColor(color, text, null);
        }

        public static void WriteLineColor(ConsoleColor color, string text, params object[] args)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, args);
            Console.ForegroundColor = fg;
        }
    }
}
