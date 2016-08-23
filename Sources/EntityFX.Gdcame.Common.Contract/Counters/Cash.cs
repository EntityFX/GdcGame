using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Counters
{
    [DataContract]
    public class Cash
    {
        [DataMember]
        public decimal CashOnHand { get; set; }

        [DataMember]
        public decimal TotalEarned { get; set; }

        [DataMember]
        public CounterBase[] Counters { get; set; }
    }
}