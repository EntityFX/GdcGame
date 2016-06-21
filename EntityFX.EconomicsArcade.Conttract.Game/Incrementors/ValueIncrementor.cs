namespace EntityFX.EconomicsArcade.Contract.Game.Incrementors
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
