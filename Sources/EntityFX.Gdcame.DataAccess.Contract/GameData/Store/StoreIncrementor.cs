using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Incrementors;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreIncrementor
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public IncrementorTypeEnum Type { get; set; }
        [DataMember]
        public int Value { get; set; }
    }
}