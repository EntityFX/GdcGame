namespace EntityFX.Gdcame.Contract.MainServer.Statistics
{
    using System;
    

    
    public class GamePerformanceInfo
    {
        
        public TimeSpan CalculationsPerCycle { get; set; }
        
        public TimeSpan PersistencePerCycle { get; set; }
    }
}