using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Presentation.Model
{
    [DataContract]
    public class GenericCounterModel : CounterModelBase
    {
        [DataMember]
        public decimal Bonus { get; set; }
        [DataMember]
        public int BonusPercentage { get; set; }
        [DataMember]
        public int Inflation { get; set; }
        [DataMember]
        public decimal SubValue { get; set; }
    }
}