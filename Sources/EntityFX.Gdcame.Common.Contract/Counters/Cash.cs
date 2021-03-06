﻿using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Counters
{
    [DataContract]
    public class Cash
    {
        [DataMember]
        public decimal OnHand { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public CounterBase[] Counters { get; set; }
    }
}