﻿using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Items
{
    [DataContract]
    public class CustomRuleInfo
    {
        [DataMember]
        public int CustomRuleId { get; set; }

        [DataMember]
        public int FundsDriverId { get; set; } 
        
        [DataMember]
        public int? CurrentIndex { get; set; } 
    }
}