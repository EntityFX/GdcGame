using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreDelayedCounter : StoreCounterBase
    {

        [DataMember]
        public decimal DelayedValue { get; set; }

        [DataMember]
        public int SecondsRemaining { get; set; }
    }
}