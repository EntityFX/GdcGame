//

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Items;

    //
    public class BuyFundDriverResult
    {
        //
        public Cash ModifiedCash { get; set; }

        //
        public Item ModifiedItem { get; set; }
    }
}