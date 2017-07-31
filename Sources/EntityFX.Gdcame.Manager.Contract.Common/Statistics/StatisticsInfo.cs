using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.Common.Statistics
{
    [DataContract]
    public class StatisticsInfo
    {
        [DataMember]
        public TimeSpan ServerUptime { get; set; }
        [DataMember]
        public DateTime ServerStartDateTime { get; set; }
        [DataMember]
        public PerformanceInfo PerformanceInfo { get; set; }
        [DataMember]
        public ResourcesUsageInfo ResourcesUsageInfo { get; set; }
        [DataMember]
        public SystemInfo SystemInfo { get; set; }
    }
}