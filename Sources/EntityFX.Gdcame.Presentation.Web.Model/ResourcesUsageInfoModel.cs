using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Application.Contract.Model
{
    public class ResourcesUsageInfoModel
    {
        public long MemoryUsed { get; set; }
        public short CpuUsed { get; set; }
    }
}