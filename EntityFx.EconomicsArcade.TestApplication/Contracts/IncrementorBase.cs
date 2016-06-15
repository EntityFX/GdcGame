using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication.Contracts
{
    public abstract class IncrementorBase
    {
        public IncrementorTypeEnum IncrementorType { get; private set; }

        protected abstract IncrementorTypeEnum GetIncrementorType();

        public int Value { get; set; }

        public IncrementorBase()
        {
            IncrementorType = GetIncrementorType();
        }

        public IncrementorBase(int value)   : this()
        {
            Value = value;
        }
    }
}
