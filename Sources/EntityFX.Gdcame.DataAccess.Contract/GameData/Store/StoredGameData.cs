using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
{
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