using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Incrementors
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