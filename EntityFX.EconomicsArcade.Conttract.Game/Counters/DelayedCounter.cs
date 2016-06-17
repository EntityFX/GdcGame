﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public class DelayedCounter : CounterBase
    {
        public decimal UnlockValue { get; set; }

        public int SecondsToAchieve { get; set; }
    }
}
