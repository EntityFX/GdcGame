﻿using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicArcade.Engine.GameEngine.CustomRules
{
    public class DelayedCounterCustomRule : ICustomRule
    {
        public void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo)
        {
            var delayedCounters = game.FundsCounters.Counters.Values.OfType<DelayedCounter>();
            foreach (var delayedCounter in delayedCounters.Where(delayedCounter => delayedCounter.SecondsToAchieve >= 1800))
            {
                delayedCounter.SecondsToAchieve -= 300;
            }
        }

        public int Id { get; set; }
    }
}