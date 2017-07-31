using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Items;

    [DataContract]
    public class BuyFundDriverResult
    {
        [DataMember]
        public Cash ModifiedCash { get; set; }

        [DataMember]
        public Item ModifiedItem { get; set; }
    }
}