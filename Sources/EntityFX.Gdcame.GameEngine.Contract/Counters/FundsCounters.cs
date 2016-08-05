using System.Collections.Generic;

namespace EntityFX.Gdcame.GameEngine.Contract.Counters
{
    public class FundsCounters
    {
        public decimal CurrentFunds { get; set; }

        public decimal TotalFunds { get; set; }

        public CounterBase RootCounter { get; set; }

        public IDictionary<int, CounterBase> Counters { get; set; }
    }
}
