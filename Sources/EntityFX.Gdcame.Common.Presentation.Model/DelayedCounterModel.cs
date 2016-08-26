using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Presentation.Model
{
    [DataContract]
    public class DelayedCounterModel : CounterModelBase
    {
        [DataMember]
        public int SecondsRemaining { get; set; }
        [DataMember]
        public decimal UnlockValue { get; set; }
    }
}