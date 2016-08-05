using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Counters
{
    [DataContract]
    public class FundsCounters
    {
        [DataMember]
        public decimal CurrentFunds { get; set; }
        [DataMember]
        public decimal TotalFunds { get; set; }
        [DataMember]
        public CounterBase[] Counters { get; set; } 
    }
}