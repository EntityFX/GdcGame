using System.Linq;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Funds;

namespace EntityFX.Gdcame.GameEngine.CustomRules
{
    public class ReduceFundDriverPriceCustomRule : ICustomRule
    {
        private const int REDUCE_TIMES = 3;
        public void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo)
        {
            if (customRuleInfo.CurrentIndex == null)
            {
                customRuleInfo.CurrentIndex = game.FundsDrivers.First().Key;
            }
            game.FundsDrivers[customRuleInfo.CurrentIndex.Value].CurrentValue /= REDUCE_TIMES;
            game.ModifiedFundsDrivers[customRuleInfo.CurrentIndex.Value] = game.FundsDrivers[customRuleInfo.CurrentIndex.Value];
            var nextItem = game.FundsDrivers.FirstOrDefault(_ => _.Key > customRuleInfo.CurrentIndex.Value);
            customRuleInfo.CurrentIndex = nextItem.Key != 0 ? nextItem.Key : game.FundsDrivers.First().Key;
            customRuleInfo.CurrentIndex = customRuleInfo.CurrentIndex;
        }

        public int Id { get; set; }
    }
}