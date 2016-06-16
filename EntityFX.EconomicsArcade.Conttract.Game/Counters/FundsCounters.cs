using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public class FundsCounters
    {
        public decimal CurrentFunds { get; set; }

        public decimal TotalFunds { get; set; }

        public CounterBase RootCounter { get; set; }

        public IDictionary<int, CounterBase> Counters { get; set; }
    }
}
