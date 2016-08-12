using System.Linq;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Items;

namespace EntityFX.Gdcame.GameEngine.CustomRules
{
    public class IncreaseFundDriverIncrementorsCustomRule : ICustomRule
    {
        private const int INCREASE_TIMES = 2;

        public void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo)
        {
            if (customRuleInfo.CurrentIndex == null)
            {
                customRuleInfo.CurrentIndex = game.Items.First().Key;
            }
            var incrementors = game.Items[customRuleInfo.CurrentIndex.Value].Incrementors.Where(_ => _.Key != 0);

            foreach (var incrementor in incrementors)
            {
                incrementor.Value.Value *= INCREASE_TIMES;
            }
            game.ModifiedFundsDrivers[customRuleInfo.CurrentIndex.Value] = game.Items[customRuleInfo.CurrentIndex.Value];
            var nextItem = game.Items.FirstOrDefault(_ => _.Key > customRuleInfo.CurrentIndex.Value);
            customRuleInfo.CurrentIndex = nextItem.Key != 0 ? nextItem.Key : game.Items.First().Key;
            customRuleInfo.CurrentIndex = customRuleInfo.CurrentIndex;
        }

        public int Id { get; set; }
    }
}