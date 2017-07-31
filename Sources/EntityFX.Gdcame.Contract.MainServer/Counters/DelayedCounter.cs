namespace EntityFX.Gdcame.Contract.MainServer.Counters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DelayedCounter : CounterBase
    {
        [DataMember]
        public decimal UnlockValue { get; set; }

        [DataMember]
        public int MiningTimeSeconds { get; set; }

        [DataMember]
        public int SecondsRemaining { get; set; }
    }
}