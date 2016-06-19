using System.Runtime.Serialization;
using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager.Counters;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager.Funds;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager
{
    [DataContract]
    public class GameData
    {
        [DataMember]
        public FundsDriver[] FundsDrivers { get; set; }
        [DataMember]
        public FundsCounters Counters { get; set; }
    }
}