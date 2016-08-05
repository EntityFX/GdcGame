using System.Collections.Generic;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;

namespace EntityFX.Gdcame.GameEngine.Contract.Funds
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

        public CustomRuleInfo CustomRuleInfo { get; set; }

        public IDictionary<int, IncrementorBase> Incrementors { get; set; }
    }
}
