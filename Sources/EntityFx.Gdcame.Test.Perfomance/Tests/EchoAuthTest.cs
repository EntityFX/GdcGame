using System;
using EntityFx.Gdcame.Test.PerfomanceFramework;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFx.Gdcame.Test.Perfomance.Tests
{
    public class EchoAuthTest : TestBase, ITest
    {
        public EchoAuthTest()
        {
            Name = "Echo with registration and auth";
        }

        protected override TestActionResultItem[] RunImplementation(RunTestSubOptions options)
        {
            Uri[] serviceAddressList = GetServers(options);


            var performanceTester = new PerfomanceTester(serviceAddressList, Logger, options.Parallelism);
            var echoWithAuthResults = performanceTester.TestEchoAuth(options.RequestsCount, RandomString(15), options.Concurrent);


            return new[] { GetTestActionResultItem("Test echo with auth", echoWithAuthResults.Item1, options.RequestsCount) };
        }
    }
}