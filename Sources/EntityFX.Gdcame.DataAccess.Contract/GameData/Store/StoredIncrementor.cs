using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
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