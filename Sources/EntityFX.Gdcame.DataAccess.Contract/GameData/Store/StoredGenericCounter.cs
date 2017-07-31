namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StoredGenericCounter : StoredCounterBase
    {
        [DataMember]
        public int BonusPercent { get; set; }

        [DataMember]
        public int Inflation { get; set; }

        [DataMember]
        public int CurrentSteps { get; set; }
    }
}