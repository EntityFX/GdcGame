using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Funds;

namespace EntityFX.Gdcame.Manager.Contract.GameManager
{
    [DataContract]
    public class BuyFundDriverResult
    {
        [DataMember]
        public FundsCounters ModifiedFundsCounters { get; set; }
        [DataMember]
        public FundsDriver ModifiedFundsDriver { get; set; }
    }

}