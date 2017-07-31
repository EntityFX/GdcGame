namespace EntityFX.Gdcame.Kernel.Contract.Counters
{
    public class CounterBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal SubValue { get; set; }

        public virtual decimal Value
        {
            get { return this.SubValue; }
        }

        public bool IsUsedInAutoStep { get; set; }
    }
}