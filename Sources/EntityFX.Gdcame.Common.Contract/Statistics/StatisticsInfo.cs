namespace EntityFX.Gdcame.Contract.Common.Statistics
{
    using System;
    

    
    public class StatisticsInfo
    {
        
        public TimeSpan ServerUptime { get; set; }
        
        public DateTime ServerStartDateTime { get; set; }

        
        public ResourcesUsageInfo ResourcesUsageInfo { get; set; }
        
        public SystemInfo SystemInfo { get; set; }

        
        public int ActiveSessionsCount { get; set; }
    }
}