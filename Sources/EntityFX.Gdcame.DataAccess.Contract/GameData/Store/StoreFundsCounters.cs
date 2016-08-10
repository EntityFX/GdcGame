using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreFundsCounters
    {
        [DataMember]
        public decimal CurrentFunds { get; set; }
        [DataMember]
        public decimal TotalFunds { get; set; }
        [DataMember]
        public StoreCounterBase[] Counters { get; set; }
    }
}