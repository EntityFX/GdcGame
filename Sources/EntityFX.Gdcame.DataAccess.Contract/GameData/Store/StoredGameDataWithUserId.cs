using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
{
    [DataContract]
    public class StoredGameDataWithUserId
    {
        [DataMember]
        public StoredGameData StoredGameData { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public DateTime CreateDateTime { get; set; }
        [DataMember]
        public DateTime? UpdateDateTime { get; set; }
    }
}