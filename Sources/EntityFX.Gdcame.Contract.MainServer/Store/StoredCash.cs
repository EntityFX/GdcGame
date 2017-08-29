namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StoredCash
    {
        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public decimal TotalEarned { get; set; }

        [DataMember]
        public StoredCounterBase[] Counters { get; set; }
    }
}