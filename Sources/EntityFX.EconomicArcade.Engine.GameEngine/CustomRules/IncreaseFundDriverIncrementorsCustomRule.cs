using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;

namespace EntityFX.EconomicArcade.Engine.GameEngine.CustomRules
{
    public class IncreaseFundDriverIncrementorsCustomRule : ICustomRule
    {
        private const int INCREASE_TIMES = 2;
        private int? _fundsDriverIndex;

        public void PerformRuleWhenBuyFundDriver(IGame game)
        {
            if (_fundsDriverIndex == null)
            {
                _fundsDriverIndex = game.FundsDrivers.First().Key;
            }
            var incrementors = game.FundsDrivers[_fundsDriverIndex.Value].Incrementors.Where(_ => _.Key != 0);

            foreach (var incrementor in incrementors)
            {
                incrementor.Value.Value *= INCREASE_TIMES;
            }
            var nextItem = game.FundsDrivers.FirstOrDefault(_ => _.Key > _fundsDriverIndex.Value);
            _fundsDriverIndex = nextItem.Key != 0 ? nextItem.Key : game.FundsDrivers.First().Key;
        }
    }
}