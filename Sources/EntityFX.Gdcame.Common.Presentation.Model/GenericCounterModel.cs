namespace EntityFX.Gdcame.Common.Presentation.Model
{
    public class GenericCounterModel : CounterModelBase
    {
        public decimal Bonus { get; set; }
        public int BonusPercentage { get; set; }
        public int Inflation { get; set; }
        public decimal SubValue { get; set; }
    }
}
