namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class DelayedCounterModel : CounterModelBase
    {
        public int SecondsRemaining { get; set; }

        public decimal UnlockValue { get; set; }
    }
}
