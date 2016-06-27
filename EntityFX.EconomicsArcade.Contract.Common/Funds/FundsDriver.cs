using System.Collections.Generic;
using System.Runtime.Serialization;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;

namespace EntityFX.EconomicsArcade.Contract.Common.Funds
{
    [DataContract]
    public class FundsDriver
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal InitialValue { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public int InflationPercent { get; set; }
        [DataMember]
        public int InflationSteps { get; set; }
        [DataMember]
        public decimal UnlockValue { get; set; }
        [DataMember]
        public int BuyCount { get; set; }
        [DataMember]
        public IDictionary<int, Incrementor> Incrementors { get; set; } 
    }
}