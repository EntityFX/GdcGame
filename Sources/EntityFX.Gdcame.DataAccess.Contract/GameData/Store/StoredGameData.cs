namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StoredGameData
    {
        [DataMember]
        public StoredItem[] Items { get; set; }

        [DataMember]
        public StoredCash Cash { get; set; }

        [DataMember]
        public int ManualStepsCount { get; set; }

        [DataMember]
        public int AutomatedStepsCount { get; set; }
    }
}