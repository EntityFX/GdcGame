namespace EntityFX.Gdcame.Contract.MainServer.Store
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