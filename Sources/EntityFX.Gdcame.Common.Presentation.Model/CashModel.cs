using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    //[DataContract]
    public class CashModel
    {
        //[DataMember]
        public decimal OnHand { get; set; }
        //[DataMember]
        public decimal TotalEarned { get; set; }
        //[DataMember]
        public CounterModelBase[] Counters { get; set; }
    }
}