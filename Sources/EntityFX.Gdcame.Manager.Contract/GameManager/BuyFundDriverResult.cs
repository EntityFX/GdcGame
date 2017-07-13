using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    [DataContract]
    public class BuyFundDriverResult
    {
        [DataMember]
        public Cash ModifiedCash { get; set; }

        [DataMember]
        public Item ModifiedItem { get; set; }
    }
}