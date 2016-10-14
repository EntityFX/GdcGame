using System.Collections.Generic;
using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using System;

namespace EntityFX.Gdcame.Common.Contract.Items
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal InitialValue { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int InflationPercent { get; set; }

        [DataMember]
        public int InflationSteps { get; set; }

        [DataMember]
        public decimal UnlockValue { get; set; }

        [DataMember]
        public int Bought { get; set; }

        [DataMember]
        public bool IsUnlocked { get; set; }

        [DataMember]
        public string Picture { get; set; }

        [DataMember]
        public CustomRuleInfo CustomRuleInfo { get; set; }

        [DataMember]
        public Incrementor[] Incrementors { get; set; }
    }
}