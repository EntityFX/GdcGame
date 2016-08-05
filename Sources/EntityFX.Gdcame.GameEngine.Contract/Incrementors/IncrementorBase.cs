namespace EntityFX.Gdcame.GameEngine.Contract.Incrementors
{
    public abstract class IncrementorBase
    {
        public IncrementorTypeEnum IncrementorType { get; private set; }

        protected abstract IncrementorTypeEnum GetIncrementorType();

        public int Value { get; set; }

        private IncrementorBase()
        {
            IncrementorType = GetIncrementorType();
        }

        protected IncrementorBase(int value)   : this()
        {
            Value = value;
        }
    }
}
