using System.Collections.Generic;
using System.Reflection;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IRuntimeHelper
    {
        string GetRuntimeName();

        string GetOsName();

        IEnumerable<Assembly> GetLoadedAssemblies();

        string GetRuntimeInfo();

        long GetTotalMemoryInMb();

        float GetCpuUsage();

        float GetAvailablememoryInMb();

        float GetMemoryUsageInMb();

        bool IsRunningOnMono();
    }
}