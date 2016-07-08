using System.Runtime.Serialization;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager
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