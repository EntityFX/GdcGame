namespace EntityFX.Gdcame.Kernel.Contract
{
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class BuyItemResult
    {
        public GameCash ModifiedGameCash { get; set; }

        public Item ModifiedItem { get; set; }
    }
}