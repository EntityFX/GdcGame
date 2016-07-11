namespace EntityFX.EconomicsArcade.Contract.Game.Counters
{
    public class CounterBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal SubValue { get; set; }

        public virtual decimal Value
        {
            get
            {
                return SubValue;
            }
        }

        public bool IsUsedInAutoStep { get; set; }
    }
}
