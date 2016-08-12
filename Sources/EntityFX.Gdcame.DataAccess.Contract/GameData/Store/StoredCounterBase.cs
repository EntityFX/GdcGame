using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    [KnownType(typeof(StoredGenericCounter))]
    [KnownType(typeof(StoredSingleCounter))]
    [KnownType(typeof(StoredDelayedCounter))]
    public class StoredCounterBase
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Value { get; set; }
    }
}