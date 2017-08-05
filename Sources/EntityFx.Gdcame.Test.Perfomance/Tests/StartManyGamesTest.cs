using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.Gdcame.Test.Perfomance.Tests
{
    using EntityFx.Gdcame.Test.PerfomanceFramework;

    class StartManyGamesTest : TestBase, ITest
    {
        public StartManyGamesTest()
        {
            Name = "Start many games with registration";
        }

        protected override TestActionResultItem[] RunImplementation(RunTestSubOptions options)
        {
            Uri[] serviceAddressList = GetServers(options);


            var performanceTester = new PerfomanceTester(serviceAddressList, Logger, options.Parallelism);
            var echoWithAuthResults = performanceTester.TestStartManyGames(options.RequestsCount, RandomString(15));


            return new[] { GetTestActionResultItem("Test echo with auth", echoWithAuthResults.Item1, options.RequestsCount) };
        }
    }
}

