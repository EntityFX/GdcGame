namespace EntityFX.Gdcame.Kernel.CustomRules
{
    using System.Linq;

    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class DelayedCounterCustomRule : ICustomRule
    {
        public void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo)
        {
            var delayedCounters = game.GameCash.Counters.OfType<DelayedCounter>();
            foreach (
                var delayedCounter in delayedCounters.Where(delayedCounter => delayedCounter.SecondsToAchieve >= 1800))
            {
                delayedCounter.SecondsToAchieve -= 300;
            }
        }

        public int Id { get; set; }
    }
}