using System.Collections.Generic;

namespace EntityFX.Gdcame.GameEngine.Contract.Counters
{
    public class GameCash
    {
        public decimal CashOnHand { get; set; }

        public decimal TotalEarned { get; set; }

        public CounterBase RootCounter { get; set; }

        public CounterBase[] Counters { get; set; }
    }
}