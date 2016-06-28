namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class GenericCounterModel : CounterModelBase
    {
        public decimal Bonus { get; set; }
        public int BonusPercentage { get; set; }
        public int Inflation { get; set; }
        public bool UseInAutoSteps { get; set; }
        public decimal SubValue { get; set; }
    }
}
