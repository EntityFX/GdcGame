namespace EntityFX.Gdcame.Contract.Common.Statistics
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResourcesUsageInfo
    {
        [DataMember]
        public float MemoryAvailable { get; set; }
        [DataMember]
        public float MemoryUsedByProcess { get; set; }
        [DataMember]
        public float CpuUsed { get; set; }
    }
}