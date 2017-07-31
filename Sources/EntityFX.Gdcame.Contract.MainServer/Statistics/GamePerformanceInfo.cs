namespace EntityFX.Gdcame.Contract.MainServer.Statistics
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class GamePerformanceInfo
    {
        [DataMember]
        public TimeSpan CalculationsPerCycle { get; set; }
        [DataMember]
        public TimeSpan PersistencePerCycle { get; set; }
    }
}