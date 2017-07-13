using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    [DataContract]
    public class StatisticsInfo
    {
        [DataMember]
        public int ActiveSessionsCount { get; set; }
        [DataMember]
        public int ActiveGamesCount { get; set; }
        [DataMember]
        public int RegistredUsersCount { get; set; }
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