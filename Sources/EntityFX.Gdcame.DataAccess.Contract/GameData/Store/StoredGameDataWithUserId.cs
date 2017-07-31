namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store
{
    using System;
    using System.Runtime.Serialization;

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