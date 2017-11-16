using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace EntityFX.Gdcame.Infrastructure
{
    public class RuntimeHelper
    {
        private static readonly PerformanceCounter _cpuCounter = new PerformanceCounter
        {
            CategoryName = "Processor",
            CounterName = "% Processor Time",
            InstanceName = "_Total"
        };

        private PerformanceCounter _ramCounter = new PerformanceCounter(
                "Memory"
                , "Available MBytes"
                , true
            );

        public static class WindowsPerformanceInfo
        {
            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

            [StructLayout(LayoutKind.Sequential)]
            public struct PerformanceInformation
            {
                public int Size;
                public IntPtr CommitTotal;
                public IntPtr CommitLimit;
                public IntPtr CommitPeak;
                public IntPtr PhysicalTotal;
                public IntPtr PhysicalAvailable;
                public IntPtr SystemCache;
                public IntPtr KernelTotal;
                public IntPtr KernelPaged;
                public IntPtr KernelNonPaged;
                public IntPtr PageSize;
                public int HandlesCount;
                public int ProcessCount;
                public int ThreadCount;
            }

            public static Int64 GetPhysicalAvailableMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }

            public static Int64 GetTotalMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }
        }

        /// <summary>
        /// FreeCSharp: quick implementation of free command (kind of) using C#
        /// </summary>
        public class UnixPerformanceInfo
        {
            public long MemTotal { get; private set; }
            public long MemFree { get; private set; }
            public long Buffers { get; private set; }
            public long Cached { get; private set; }

            public void GetValues()
            {
                string[] memInfoLines = File.ReadAllLines(@"/proc/meminfo");

                MemInfoMatch[] memInfoMatches =
                {
                new MemInfoMatch(@"^Buffers:\s+(\d+)", value => Buffers = Convert.ToInt64(value)),
                new MemInfoMatch(@"^Cached:\s+(\d+)", value => Cached = Convert.ToInt64(value)),
                new MemInfoMatch(@"^MemFree:\s+(\d+)", value => MemFree = Convert.ToInt64(value)),
                new MemInfoMatch(@"^MemTotal:\s+(\d+)", value => MemTotal = Convert.ToInt64(value))
            };

                foreach (string memInfoLine in memInfoLines)
                {
                    foreach (MemInfoMatch memInfoMatch in memInfoMatches)
                    {
                        Match match = memInfoMatch.regex.Match(memInfoLine);
                        if (match.Groups[1].Success)
                        {
                            string value = match.Groups[1].Value;
                            memInfoMatch.updateValue(value);
                        }
                    }
                }
            }

            public class MemInfoMatch
            {
                public Regex regex;
                public Action<string> updateValue;

                public MemInfoMatch(string pattern, Action<string> update)
                {
                    this.regex = new Regex(pattern, RegexOptions.Compiled);
                    this.updateValue = update;
                }
            }
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public static string GetRuntimeName()
        {
            var infoBuilder = new StringBuilder();
            if (IsRunningOnMono())
            {
                Type type = Type.GetType("Mono.Runtime");
                MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
                infoBuilder.Append(string.Format("Mono {0}, version: {1}",
                    displayName.Invoke(null, null), Environment.Version));
            }
            else
            {
                infoBuilder.Append(string.Format(".Net Framework {0}", Environment.Version));
            }
            return infoBuilder.ToString();
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
                infoBuilder.AppendLine(string.Format("Runtime: Mono {0}, version: {1}",
                    displayName.Invoke(null, null), Environment.Version));
            }
            else
            {
                infoBuilder.AppendLine(string.Format("Runtime: .Net Framework {0}", Environment.Version));
            }
            return infoBuilder.ToString();
        }

        public static long GetTotalMemoryInMb()
        {
            if (IsRunningOnMono())
            {
                var pc = new PerformanceCounter("Mono Memory", "Total Physical Memory");
                return pc.RawValue / 1024 / 1024;
            }
            else
            {
                return WindowsPerformanceInfo.GetTotalMemoryInMiB();
            }

        }

        public static float GetCpuUsage()
        {
            return _cpuCounter.NextValue();
        }

        public static float GetAvailablememoryInMb()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var upi = new UnixPerformanceInfo();
                upi.GetValues();
                return upi.MemFree / 1024;
            }
            else
            {
                PerformanceCounter _ramCounter = new PerformanceCounter();

                _ramCounter = new PerformanceCounter(
                    "Memory"
                    , "Available MBytes"
                    , true
                );
                return _ramCounter.NextValue();
            }

        }
    }
}