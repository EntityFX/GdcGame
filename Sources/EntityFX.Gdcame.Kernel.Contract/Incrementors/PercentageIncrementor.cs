namespace EntityFX.Gdcame.Kernel.Contract.Incrementors
{
    public class PercentageIncrementor : IncrementorBase
    {
        protected override IncrementorTypeEnum GetIncrementorType()
        {
            return IncrementorTypeEnum.PercentageIncrementor;
        }
    }
}