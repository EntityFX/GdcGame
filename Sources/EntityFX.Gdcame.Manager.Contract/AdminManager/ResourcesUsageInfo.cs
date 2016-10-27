using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.AdminManager
{
    [DataContract]
    public class ResourcesUsageInfo
    {
        [DataMember]
        public long MemoryUsed { get; set; }
        [DataMember]
        public short CpuUsed { get; set; }
    }
}