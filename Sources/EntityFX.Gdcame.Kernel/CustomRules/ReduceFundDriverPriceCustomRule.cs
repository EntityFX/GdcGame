namespace EntityFX.Gdcame.Kernel.CustomRules
{
    using System.Linq;

    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class ReduceFundDriverPriceCustomRule : ICustomRule
    {
        private const int REDUCE_TIMES = 3;

        public void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo)
        {
            if (customRuleInfo.CurrentIndex == null)
            {
                customRuleInfo.CurrentIndex =0;
            }
            game.Items[customRuleInfo.CurrentIndex.Value].Price /= REDUCE_TIMES;
            game.ModifiedFundsDrivers[customRuleInfo.CurrentIndex.Value] = game.Items[customRuleInfo.CurrentIndex.Value];
            var nextItem = game.Items.FirstOrDefault(_ => _.Id> customRuleInfo.CurrentIndex.Value);
            customRuleInfo.CurrentIndex = nextItem.Id != 0 ? nextItem.Id : 0;
            customRuleInfo.CurrentIndex = customRuleInfo.CurrentIndex;
        }

        public int Id { get; set; }
    }
}