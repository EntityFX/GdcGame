using System;
using EntityFx.Gdcame.Test.PerfomanceFramework;
using EntityFX.Gdcame.Infrastructure.Common;
using PortableLog.NLog;

namespace EntityFx.Gdcame.Test.Perfomance.Tests
{
    public class EchoTest : TestBase, ITest
    {
        public EchoTest()
        {
            Name = "Echo";
        }

        protected override TestActionResultItem[] RunImplementation(RunTestSubOptions options)
        {
            Uri[] serviceAddressList = GetServers(options);

            var performanceTester = new PerfomanceTester(serviceAddressList, Logger, options.Parallelism);
            var echoPerformanceResults = performanceTester.TestEcho(options.RequestsCount, RandomString(15), options.Concurrent);

            return new[] { GetTestActionResultItem("Test echo", echoPerformanceResults.Item1, options.RequestsCount)};
        }
    }
}