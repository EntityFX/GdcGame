using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Presentation.Model
{
    [DataContract]
    [KnownType(typeof(GenericCounterModel))]
    [KnownType(typeof(SingleCounterModel))]
    [KnownType(typeof(DelayedCounterModel))]
    public class CounterModelBase
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public int Type { get; set; }
    }


}