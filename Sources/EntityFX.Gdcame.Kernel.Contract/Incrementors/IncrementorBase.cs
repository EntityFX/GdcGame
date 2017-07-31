namespace EntityFX.Gdcame.Kernel.Contract.Incrementors
{
    public abstract class IncrementorBase
    {
        public void Init(int value)
        {
            this.IncrementorType = this.GetIncrementorType();
            this.Value = value;
        }

        public IncrementorTypeEnum IncrementorType { get; private set; }

        public int Value { get; set; }

        protected abstract IncrementorTypeEnum GetIncrementorType();
    }
}