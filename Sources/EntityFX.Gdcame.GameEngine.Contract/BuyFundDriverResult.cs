using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Funds;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public class BuyFundDriverResult
    {
        public FundsCounters ModifiedFundsCounters { get; set; }

        public FundsDriver ModifiedFundsDriver { get; set; }
    }
}