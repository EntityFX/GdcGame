namespace EntityFX.Gdcame.Contract.MainServer.Counters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Cash
    {
        [DataMember]
        public decimal OnHand { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public CounterBase[] Counters { get; set; }
    }
}