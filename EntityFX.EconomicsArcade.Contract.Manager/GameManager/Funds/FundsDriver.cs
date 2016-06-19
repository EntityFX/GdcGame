using System.Collections.Generic;
using System.Runtime.Serialization;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager.Incrementors;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager.Funds
{
    [DataContract]
    public class FundsDriver
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public int InflationPercent { get; set; }
        [DataMember]
        public decimal UnlockValue { get; set; }
        [DataMember]
        public int BuyCount { get; set; }
        [DataMember]
        public IDictionary<int, Incrementor> Incrementors { get; set; } 
    }
}