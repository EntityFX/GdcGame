using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public class CounterBase
    {
        public string Name { get; set; }

        public decimal SubValue { get; set; }

        public virtual decimal Value
        {
            get
            {
                return SubValue;
            }
        }

        public bool IsUsedInAutoStep { get; set; }
    }
}
