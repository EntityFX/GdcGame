using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
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