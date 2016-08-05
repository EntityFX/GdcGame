namespace EntityFX.Gdcame.GameEngine.Contract.Counters
{
    public class DelayedCounter : CounterBase
    {
        public decimal UnlockValue { get; set; }

        public int SecondsToAchieve { get; set; }

        public int SecondsRemaining { get; set; }

        public bool IsMining { get; set; }

        public override decimal Value
        {
            get
            {
                return SubValue;
            }
        }
    }
}
