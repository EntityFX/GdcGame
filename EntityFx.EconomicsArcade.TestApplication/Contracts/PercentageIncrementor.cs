using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.Contracts
{
    class PercentageIncrementor : IncrementorBase
    {
        public PercentageIncrementor(int value)
            : base(value)
        {
        }
        
        protected override IncrementorTypeEnum GetIncrementorType()
        {
            return IncrementorTypeEnum.PercentageIncrementor;
        }
    }
}
