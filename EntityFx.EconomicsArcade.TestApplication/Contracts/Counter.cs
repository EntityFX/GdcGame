using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.Contracts
{
    public class Counter
    {
        public string Name { get; set; }

        public virtual decimal Value { get; set; }

        public bool IsUsedInAutoStep { get; set; }
    }
}
