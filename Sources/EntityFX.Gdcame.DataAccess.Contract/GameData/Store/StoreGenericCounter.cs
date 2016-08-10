using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreGenericCounter : StoreCounterBase
    {
        [DataMember]
        public int BonusPercent { get; set; }
        [DataMember]
        public int Inflation { get; set; }
        [DataMember]
        public int CurrentSteps { get; set; }
    }
}