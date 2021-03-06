﻿using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
{
    [DataContract]
    public class StoredCustomRuleInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? CurrentIndex { get; set; }
    }
}