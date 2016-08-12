using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Incrementors;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoredIncrementor
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Value { get; set; }
    }
}