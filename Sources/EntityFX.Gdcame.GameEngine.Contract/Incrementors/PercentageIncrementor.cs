namespace EntityFX.Gdcame.GameEngine.Contract.Incrementors
{
    public class PercentageIncrementor : IncrementorBase
    {
        public PercentageIncrementor(int value)
            : base(value)
        {
        }
        
        protected override IncrementorTypeEnum GetIncrementorType()
        {
            return IncrementorTypeEnum.PercentageIncrementor;
        }
    }
}
