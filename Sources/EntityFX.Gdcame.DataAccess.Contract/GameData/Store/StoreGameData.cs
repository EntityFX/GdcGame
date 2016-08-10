using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreGameData
    {
        [DataMember]
        public StoreFundsDriver[] FundsDrivers { get; set; }
        [DataMember]
        public StoreFundsCounters Counters { get; set; }
        [DataMember]
        public StoreCustomRule[] CustomRules { get; set; }
        [DataMember]
        public int ManualStepsCount { get; set; }
        [DataMember]
        public int AutomaticStepsCount { get; set; }
    }
}