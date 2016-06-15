using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.Contracts
{
    class ValueIncrementor : IncrementorBase
    {
        public ValueIncrementor(int value)
            : base(value)
        {
        }
        
        protected override IncrementorTypeEnum GetIncrementorType()
        {
            return IncrementorTypeEnum.ValueIncrementor;
        }
    }
}
