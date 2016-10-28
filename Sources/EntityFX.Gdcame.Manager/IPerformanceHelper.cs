namespace EntityFX.Gdcame.Manager
{
    public interface IPerformanceHelper
    {
        float CpuUsage { get;  }

        float MemoryUsage { get; }

        float MemoryUsageByProcess { get; }
    }
}