using System.Collections.Generic;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;

namespace EntityFX.EconomicsArcade.Contract.Game.Funds
{
    public class FundsDriver
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal InitialValue { get; set; }

        public int InflationPercent { get; set; }

        public decimal UnlockValue { get; set; }

        public int BuyCount { get; set; }

        public IDictionary<int, IncrementorBase> Incrementors { get; set; }
    }
}
