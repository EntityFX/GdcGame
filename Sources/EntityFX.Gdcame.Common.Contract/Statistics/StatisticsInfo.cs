namespace EntityFX.Gdcame.Contract.Common.Statistics
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class StatisticsInfo
    {
        [DataMember]
        public TimeSpan ServerUptime { get; set; }
        [DataMember]
        public DateTime ServerStartDateTime { get; set; }

        [DataMember]
        public ResourcesUsageInfo ResourcesUsageInfo { get; set; }
        [DataMember]
        public SystemInfo SystemInfo { get; set; }

        [DataMember]
        public int ActiveSessionsCount { get; set; }
    }
}