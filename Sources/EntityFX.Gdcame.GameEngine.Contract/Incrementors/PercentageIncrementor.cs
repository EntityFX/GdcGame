namespace EntityFX.Gdcame.GameEngine.Contract.Incrementors
{
    public class PercentageIncrementor : IncrementorBase
    {
        protected override IncrementorTypeEnum GetIncrementorType()
        {
            return IncrementorTypeEnum.PercentageIncrementor;
        }
    }
}