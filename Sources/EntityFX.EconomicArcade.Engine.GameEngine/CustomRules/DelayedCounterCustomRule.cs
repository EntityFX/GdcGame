using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;

namespace EntityFX.EconomicArcade.Engine.GameEngine.CustomRules
{
    public class DelayedCounterCustomRule : ICustomRule
    {
        public void PerformRuleWhenBuyFundDriver(IGame game)
        {
            var delayedCounters = game.FundsCounters.Counters.Values.OfType<DelayedCounter>();
            foreach (var delayedCounter in delayedCounters.Where(delayedCounter => delayedCounter.SecondsToAchieve >= 1800))
            {
                delayedCounter.SecondsToAchieve -= 300;
            }
        }
    }
}