using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Funds;

namespace EntityFX.Gdcame.Common.Contract
{
    [DataContract]
    public class GameData
    {
        [DataMember]
        public FundsDriver[] FundsDrivers { get; set; }
        [DataMember]
        public FundsCounters Counters { get; set; }
        [DataMember]
        public CustomRule[] CustomRules { get; set; }
        [DataMember]
        public int ManualStepsCount { get; set; }
        [DataMember]
        public int AutomaticStepsCount { get; set; }
    }
}