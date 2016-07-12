namespace EntityFX.EconomicsArcade.Model.Common.Model
{
    public class DelayedCounterModel : CounterModelBase
    {
        public int SecondsRemaining { get; set; }

        public decimal UnlockValue { get; set; }
    }
}
