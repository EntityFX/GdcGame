using System;
using EntityFx.Gdcame.Test.PerfomanceFramework;
using EntityFX.Gdcame.Infrastructure.Common;
using PortableLog.NLog;

namespace EntityFx.Gdcame.Test.Perfomance.Tests
{
    public class RegressionTest : TestBase, ITest
    {
        public RegressionTest()
        {
            Name = "Regression Test";
        }

        protected override TestActionResultItem[] RunImplementation(RunTestSubOptions options)
        {

            Uri[] serviceAddressList = GetServers(options);
            var performanceTester = new PerfomanceTester(serviceAddressList, Logger, options.Parallelism);
            /*
            performanceTester.TestStartManyGames(options.RequestsCount, RandomString(15));
            Logger.Info("Press any key to close...");
            Console.ReadKey();

            Array.ForEach(new int[] { 10, 50, 100, 500, 1000, 5000, 10000, 50000 }, i =>
            {
                Logger.Info("Testing with {0} accounts", i);
                Logger.Info("Test: Register accounts");
                performanceTester.TestPerformanceRegisterManyAccounts(i, RandomString(15), false);
                Logger.Info("Test: Get game data");
                performanceTester.TestGetGameData(i, RandomString(15), false);
                Logger.Info("Test: Perform step");
                performanceTester.TestPerformStepAction(i, RandomString(15), false);

                Logger.Info("Test: Register accounts in parallel");
                performanceTester.TestPerformanceRegisterManyAccounts(i, RandomString(15), true);
                Logger.Info("Test: Get game data in parallel");
                performanceTester.TestGetGameData(i, RandomString(15), true);

                Logger.Info("Test: Perform step in parallel");
                performanceTester.TestPerformStepAction(i, RandomString(15), true);
                Logger.Info("\n");
            });
            Logger.Info("Press any key to close...");
            Console.ReadKey();*/
            return null;
        }
    }
}