using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreGenericCounter : StoreCounterBase
    {
        [DataMember]
        public decimal Bonus { get; set; }
        [DataMember]
        public int BonusPercentage { get; set; }
        [DataMember]
        public int Inflation { get; set; }
        [DataMember]
        public bool UseInAutoSteps { get; set; }
        [DataMember]
        public decimal SubValue { get; set; }
        [DataMember]
        public int CurrentSteps { get; set; }
        [DataMember]
        public int InflationIncreaseSteps { get; set; }
    }
}