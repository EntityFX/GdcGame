using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicArcade.Engine.GameEngine.CustomRules
{
    public class ReduceFundDriverPriceCustomRule : ICustomRule
    {
        private const int REDUCE_TIMES = 3;
        private int? _fundsDriverIndex;
        public void PerformRuleWhenBuyFundDriver(IGame game)
        {
            if (_fundsDriverIndex == null)
            {
                _fundsDriverIndex = game.FundsDrivers.First().Key;
            }
            game.FundsDrivers[_fundsDriverIndex.Value].CurrentValue /= REDUCE_TIMES;
            var nextItem = game.FundsDrivers.FirstOrDefault(_ => _.Key > _fundsDriverIndex.Value);
            _fundsDriverIndex = nextItem.Key != 0 ? nextItem.Key : game.FundsDrivers.First().Key;
        }
    }
}