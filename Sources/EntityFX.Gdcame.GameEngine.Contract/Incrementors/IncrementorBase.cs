namespace EntityFX.Gdcame.GameEngine.Contract.Incrementors
{
    public abstract class IncrementorBase
    {
        public void Init(int value)
        {
            IncrementorType = GetIncrementorType();
            Value = value;
        }

        public IncrementorTypeEnum IncrementorType { get; private set; }

        public int Value { get; set; }

        protected abstract IncrementorTypeEnum GetIncrementorType();
    }
}