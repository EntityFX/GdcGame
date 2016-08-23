namespace EntityFX.Gdcame.GameEngine.Contract.Incrementors
{
    public abstract class IncrementorBase
    {
        private IncrementorBase()
        {
            IncrementorType = GetIncrementorType();
        }

        protected IncrementorBase(int value) : this()
        {
            Value = value;
        }

        public IncrementorTypeEnum IncrementorType { get; private set; }

        public int Value { get; set; }

        protected abstract IncrementorTypeEnum GetIncrementorType();
    }
}