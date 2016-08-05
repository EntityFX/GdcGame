using System.Linq;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Funds;

namespace EntityFX.Gdcame.GameEngine.CustomRules
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