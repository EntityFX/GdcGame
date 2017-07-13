namespace EntityFX.Gdcame.Manager.Contract.Common
{
    public interface IPerformanceHelper
    {
        float CpuUsage { get;  }

        float MemoryUsage { get; }

        float MemoryUsageByProcess { get; }
    }
}