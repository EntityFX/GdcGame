namespace EntityFX.EconomicsArcade.Contract.Game.Incrementors
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
