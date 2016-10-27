using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Application.Contract.Model
{
    [DataContract]
    public class PerformanceInfoModel
    {
        [DataMember]
        public TimeSpan CalculationsPerCycle { get; set; }
        [DataMember]
        public TimeSpan PersistencePerCycle { get; set; }
    }
}