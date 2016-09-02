using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace EntityFX.Gdcame.Utils.Common
{
    public class RuntimeHelper
    {
        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public static string GetRuntimeInfo()
        {
            var infoBuilder = new StringBuilder();
            infoBuilder.AppendLine(string.Format("OS: {0}", Environment.OSVersion));
            infoBuilder.AppendLine(string.Format("CPUs: {0}", Environment.ProcessorCount));
            if (IsRunningOnMono())
            {
                Type type = Type.GetType("Mono.Runtime");
                MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
                infoBuilder.AppendLine(string.Format("Runtime: Mono {0}, environment version: {1}",
                    displayName.Invoke(null, null), Environment.Version));
            }
            else
            {
                infoBuilder.AppendLine(string.Format("Runtime: .Net Framework {0}", Environment.Version));
            }
            return infoBuilder.ToString();
        }
    }
}