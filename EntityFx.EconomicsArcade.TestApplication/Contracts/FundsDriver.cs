using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.Contracts
{
    public class FundsDriver
    {
        public string Name { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal InitialValue { get; set; }

        public int InflationPercent { get; set; }

        public decimal UnlockValue { get; set; }

        public int BuyCount { get; set; }

        public IDictionary<int, IncrementorBase> Incrementors { get; set; }
    }
}
