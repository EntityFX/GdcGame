namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IRuntimeHelper
    {
        string GetRuntimeName();

        string GetRuntimeInfo();

        long GetTotalMemoryInMb();

        float GetCpuUsage();

        float GetAvailablememoryInMb();

        float GetMemoryUsageInMb();

        bool IsRunningOnMono();
    }
}