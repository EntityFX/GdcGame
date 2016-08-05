using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Counters
{
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