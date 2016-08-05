namespace EntityFX.Gdcame.GameEngine.Contract.Incrementors
{
    public class ValueIncrementor : IncrementorBase
    {
        public ValueIncrementor(int value)
            : base(value)
        {
        }
        
        protected override IncrementorTypeEnum GetIncrementorType()
        {
            return IncrementorTypeEnum.ValueIncrementor;
        }
    }
}
