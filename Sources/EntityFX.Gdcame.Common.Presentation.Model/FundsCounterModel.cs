namespace EntityFX.Gdcame.Common.Presentation.Model
{
    public class FundsCounterModel
    {
        public decimal CurrentFunds { get; set; }
        public decimal TotalFunds { get; set; }
        public CounterModelBase[] Counters { get; set; }
    }
}
