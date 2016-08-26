using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Presentation.Model
{
    [DataContract]
    public class CashModel
    {
        [DataMember]
        public decimal OnHand { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public CounterModelBase[] Counters { get; set; }
    }
}