using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    [KnownType(typeof(StoreGenericCounter))]
    [KnownType(typeof(StoreSingleCounter))]
    [KnownType(typeof(StoreDelayedCounter))]
    public class StoreCounterBase
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public int Type { get; set; }
    }
}