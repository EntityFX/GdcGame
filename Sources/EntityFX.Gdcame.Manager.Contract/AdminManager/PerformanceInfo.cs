using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.AdminManager
{
    [DataContract]
    public class PerformanceInfo
    {
        [DataMember]
        public TimeSpan CalculationsPerCycle { get; set; }
        [DataMember]
        public TimeSpan PersistencePerCycle { get; set; }
    }
}