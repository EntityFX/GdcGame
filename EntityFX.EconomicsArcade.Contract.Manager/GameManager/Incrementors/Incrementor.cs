using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager.Incrementors
{
    [DataContract]
    public class Incrementor
    {
        [DataMember]
        public IncrementorTypeEnum IncrementorType { get; set; }
        [DataMember]
        public int Value { get; set; }
    }
}