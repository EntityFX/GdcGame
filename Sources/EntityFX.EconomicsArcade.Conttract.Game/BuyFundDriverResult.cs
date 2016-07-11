using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public class BuyFundDriverResult
    {
        public FundsCounters ModifiedFundsCounters { get; set; }

        public FundsDriver ModifiedFundsDriver { get; set; }
    }
}