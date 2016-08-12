using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Items;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public class BuyItemResult
    {
        public GameCash ModifiedGameCash { get; set; }

        public Item ModifiedItem { get; set; }
    }
}