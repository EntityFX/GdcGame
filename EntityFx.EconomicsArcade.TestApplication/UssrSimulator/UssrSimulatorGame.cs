using EntityFx.EconomicsArcade.TestApplication.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.UssrSimulator
{
    public class UssrSimulatorGame : GameBase
    {
        public decimal Communism
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.Communism].Value;
            }
        }

        public decimal Production
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.Production].Value;
            }
        }

        public decimal Taxes
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.Tax].Value;
            }
        }

        public decimal FiveYearPlan
        {
            get
            {
                return this.FundsCounters.Counters[(int)UssrCounterEnum.FiveYearPlan].Value;
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
                                new ValueIncrementor(1)
                            } ,
                        },
                        Name = "Beer"
                    }   
                },
                {
                    4,
                    new FundsDriver {
                        InitialValue = 1500,
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
                        Incrementors = new Dictionary<int, IncrementorBase> {
                            {
                                (int)UssrCounterEnum.Tax,
                                new ValueIncrementor(80)
                            } ,
                            {
                                (int)UssrCounterEnum.Communism,
                                new ValueIncrementor(10)
                            } ,
                        },
                        Name = "Hucksterism"
                    }   
                },
                {
                    7,
                    new FundsDriver {
                        InitialValue = 10000,
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
                        InitialValue = 60000,
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
                        InitialValue = 80000,
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
                        InitialValue = 100000,
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
                        InitialValue = 120000,
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
                        InitialValue = 150000,
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
                        InitialValue = 180000,
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
                        InitialValue = 200000,
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
                        InitialValue = 250000,
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
                        InitialValue = 400000,
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
                        InitialValue = 1000000,
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
                        Name = "Black Market"
                    }   
                },
            };
        }

        protected override FundsCounters GetFundsCounters()
        {
            return new FundsCounters()
            {
                Counters = new Dictionary<int, Counter>() { 
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
                            StepsToIncreaseInflation = 5000
                        } 
                    }, 
                    { 
                        (int)UssrCounterEnum.Tax, 
                        new GenericCounter {
                            Name = "Tax",
                            IsUsedInAutoStep = true
                        }   
                    },
                    { 
                        (int)UssrCounterEnum.FiveYearPlan, 
                        new DelayedCounter {
                            Name = "Five Year Plan",
                        }   
                    },
                }
            };
        }

        private object _lockObject = new { };

        protected override void PostPerformAutoStep()
        {

                DisplayGameData();

        }

        protected override void PostInitialize()
        {
            //CashFunds(1500000);
        }

        protected override void PostByFundDriver(int fundDriverId)
        {
            DisplayGameData();
        }

        private void DisplayGameData()
        {
            lock (_lockObject)
            {
                Console.Clear();
                Console.WriteLine("Funds: {0:C}; Total Funds: {1:C}", this.FundsCounters.CurrentFunds, FundsCounters.TotalFunds);
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "{1,15}: {0,12}", Communism, this.FundsCounters.Counters[(int)UssrCounterEnum.Communism].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ", Production, this.FundsCounters.Counters[(int)UssrCounterEnum.Production].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Production]).Bonus, "Bonus"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Production]).BonusPercentage);
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12}%"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Production]).Inflation, "Inflation");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Production]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}", Taxes, this.FundsCounters.Counters[(int)UssrCounterEnum.Tax].Name);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Tax]).Bonus, "Bonus"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Tax]).BonusPercentage);
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Tax]).Inflation, "Corruption");
                PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                    , ((GenericCounter)this.FundsCounters.Counters[(int)UssrCounterEnum.Tax]).Value, "Total");
                PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: {0,12}", FiveYearPlan, this.FundsCounters.Counters[(int)UssrCounterEnum.FiveYearPlan].Name);
                Console.WriteLine();
                int charIndex = 65;
                foreach (var fundsDriver in FundsDrivers)
                {
                    Console.Write("{3,2}: {0,28} {1,15:C} x{2,-4} ", fundsDriver.Value.Name, fundsDriver.Value.CurrentValue, fundsDriver.Value.BuyCount, ((char)charIndex).ToString());
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
        private static readonly object _lockObject = new { };
        
        public static void WriteColor(ConsoleColor color, string text)
        {
            WriteColor(color, text, null);
        }

        public static void WriteColor(ConsoleColor color, string text, params object[] args)
        {
            lock (_lockObject)
            {
                var fg = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.Write(text, args);
                Console.ForegroundColor = fg;
            }
        }

        public static void WriteLineColor(ConsoleColor color, string text)
        {
            WriteLineColor(color, text, null);
        }

        public static void WriteLineColor(ConsoleColor color, string text, params object[] args)
        {
            lock (_lockObject)
            {
                var fg = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(text, args);
                Console.ForegroundColor = fg;
            }
        }
    }
}
