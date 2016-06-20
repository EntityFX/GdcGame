using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Common.Counters
{
    [DataContract]
    [KnownType(typeof(GenericCounter))]
    [KnownType(typeof(SingleCounter))]
    public class CounterBase
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal Value { get; set; }
    }
}