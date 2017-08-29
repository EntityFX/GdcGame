namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StoredDelayedCounter : StoredCounterBase
    {
        [DataMember]
        public decimal DelayedValue { get; set; }

        [DataMember]
        public int SecondsRemaining { get; set; }
    }
}