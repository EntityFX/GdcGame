namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(typeof (StoredGenericCounter))]
    [KnownType(typeof (StoredSingleCounter))]
    [KnownType(typeof (StoredDelayedCounter))]
    public class StoredCounterBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }
}