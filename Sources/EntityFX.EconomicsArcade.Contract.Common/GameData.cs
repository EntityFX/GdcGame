using System.Runtime.Serialization;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;

namespace EntityFX.EconomicsArcade.Contract.Common
{
    [DataContract]
    public class GameData
    {
        [DataMember]
        public FundsDriver[] FundsDrivers { get; set; }
        [DataMember]
        public FundsCounters Counters { get; set; }
        [DataMember]
        public int ManualStepsCount { get; set; }
        [DataMember]
        public int AutomaticStepsCount { get; set; }
    }
}