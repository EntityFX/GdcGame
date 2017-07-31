using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.Common.Statistics
{
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