﻿using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Funds
{
    [DataContract]
    public class StoreCustomRuleInfo
    {
        [DataMember]
        public int CustomRuleId { get; set; }

        [DataMember]
        public int FundsDriverId { get; set; } 
        
        [DataMember]
        public int? CurrentIndex { get; set; } 
    }
}