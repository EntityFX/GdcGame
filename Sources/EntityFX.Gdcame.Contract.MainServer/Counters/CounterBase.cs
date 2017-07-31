namespace EntityFX.Gdcame.Contract.MainServer.Counters
{
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(typeof (GenericCounter))]
    [KnownType(typeof (SingleCounter))]
    [KnownType(typeof (DelayedCounter))]
    public class CounterBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public int Type { get; set; }
    }


}