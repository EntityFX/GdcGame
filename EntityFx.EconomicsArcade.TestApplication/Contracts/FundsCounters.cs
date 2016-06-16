using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.Contracts
{
    public class FundsCounters
    {
        public decimal CurrentFunds { get; set; }

        public decimal TotalFunds { get; set; }

        public Counter RootCounter { get; set; }

        public IDictionary<int, Counter> Counters { get; set; }
    }
}
