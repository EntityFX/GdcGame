namespace EntityFX.Gdcame.Kernel.Contract.Incrementors
{
    public class ValueIncrementor : IncrementorBase
    {
        protected override IncrementorTypeEnum GetIncrementorType()
        {
            return IncrementorTypeEnum.ValueIncrementor;
        }
    }
}