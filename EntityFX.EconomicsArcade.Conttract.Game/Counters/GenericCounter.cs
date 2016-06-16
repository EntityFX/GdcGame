using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public class GenericCounter : CounterBase
    {
        public decimal Bonus
        {
            get
            {
                return SubValue * BonusPercentage / 100.0m;
            }
        }

        public int BonusPercentage { get; set; }

        public int Inflation { get; set; }

        public int StepsToIncreaseInflation { get; set; }

        public decimal SubValue { get; set; }

        public override decimal Value
        {
            get
            {
                var total = SubValue + Bonus;
                return total - total * Inflation / 100;
            }
            set
            {
                SubValue = value;
            }
        }
    }
}
