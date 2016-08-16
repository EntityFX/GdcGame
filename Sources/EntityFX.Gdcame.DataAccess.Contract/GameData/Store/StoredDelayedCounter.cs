﻿using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
{
    [DataContract]
    public class StoredDelayedCounter : StoredCounterBase
    {

        [DataMember]
        public decimal DelayedValue { get; set; }

        [DataMember]
        public int SecondsRemaining { get; set; }
    }
}