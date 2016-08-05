using System.Collections.Generic;
using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Incrementors;

namespace EntityFX.Gdcame.Common.Contract.Funds
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
        public bool IsActive { get; set; }
        [DataMember]
        public string Picture { get; set; }
        [DataMember]
        public CustomRuleInfo CustomRuleInfo { get; set; }
        [DataMember]
        public IDictionary<int, Incrementor> Incrementors { get; set; } 
    }
}