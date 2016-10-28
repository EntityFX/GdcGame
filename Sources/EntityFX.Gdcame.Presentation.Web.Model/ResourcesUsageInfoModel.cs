using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Application.Contract.Model
{
    public class ResourcesUsageInfoModel
    {
        public float MemoryAvailable { get; set; }
        public float MemoryUsedByProcess { get; set; }
        public float CpuUsed { get; set; }
    }
}