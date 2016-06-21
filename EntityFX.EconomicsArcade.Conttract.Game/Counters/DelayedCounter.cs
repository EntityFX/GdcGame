namespace EntityFX.EconomicsArcade.Contract.Game.Counters
{
    public class DelayedCounter : CounterBase
    {
        public decimal UnlockValue { get; set; }

        public int SecondsToAchieve { get; set; }
    }
}
