﻿namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StoredIncrementor
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Value { get; set; }
    }
}