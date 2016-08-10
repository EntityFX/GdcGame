using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreDelayedCounter : StoreCounterBase
    {
        [DataMember]
        public decimal UnlockValue { get; set; }

        [DataMember]
        public int MiningTimeSeconds { get; set; }

        [DataMember]
        public int SecondsRemaining { get; set; }
    }
}