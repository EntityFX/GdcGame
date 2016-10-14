using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.WebApi.Models;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
using PortableLog.NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFx.Gdcame.Test.PerfomanceFramework;

namespace EntityFx.Gdcame.Test.Perfomance
{
    class Program
    {


        static void Main(string[] args)
        {
            var serviceAddress = "http://localhost:9001/";
            var countRequests = 10000;
            if (args.Length > 0)
            {
                serviceAddress = args[0];
            }

            if (args.Length > 1)
            {
                countRequests = Convert.ToInt32(args[1]);
            }

            var logger = new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")));

            var performanceTester = new PerfomanceTester(new Uri(serviceAddress), logger);

            performanceTester.TestStartManyGames(countRequests, RandomString(15));
            logger.Info("Press any key to close...");
            Console.ReadKey();

            Array.ForEach(new int[] { 10, 50, 100, 500, 1000, 5000, 10000, 50000 }, i =>
            {
                logger.Info("Testing with {0} accounts", i);
                logger.Info("Test: Register accounts");
                performanceTester.TestPerformanceRegisterManyAccounts(i, RandomString(15), false);
                logger.Info("Test: Get game data");
                performanceTester.TestGetGameData(i, RandomString(15), false);
                logger.Info("Test: Perform step");
                performanceTester.TestPerformStepAction(i, RandomString(15), false);

                logger.Info("Test: Register accounts in parallel");
                performanceTester.TestPerformanceRegisterManyAccounts(i, RandomString(15), true);
                logger.Info("Test: Get game data in parallel");
                performanceTester.TestGetGameData(i, RandomString(15), true);

                logger.Info("Test: Perform step in parallel");
                performanceTester.TestPerformStepAction(i, RandomString(15), true);
                logger.Info("\n");
            });
            logger.Info("Press any key to close...");
            Console.ReadKey();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }



    class PerfomanceTester
    {
        public const int ParallelismFactor = 256;

        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string DefaultPassword = "P@ssw0rd";

        private readonly Uri _serviceUri;

        private readonly ILogger _logger;

        private readonly List<TestInfo> _testsInfo = new List<TestInfo>();

        private readonly PerformanceApi _performanceApi;

        public IReadOnlyList<TestInfo> TestInfo
        {
            get
            {
                return _testsInfo;
            }
        }



        public PerfomanceTester(Uri serviceUri, ILogger logger)
        {
            _serviceUri = serviceUri;
            _logger = logger;
            _performanceApi = new PerformanceApi(serviceUri, logger);
        }

        public void TestStartManyGames(int countAccounts, string accounLoginPrefix)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = _performanceApi.LoginAsAdmin().Result;

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Restart();
            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, true);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);

            _logger.Info("\tStart login {0} accounts in parallel", countAccounts);
            ClientConnectionInfo[] clientLogins = _performanceApi.LoginManyClients(countAccounts, accounLoginPrefix);
            _logger.Info("\tDone login {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            sw.Restart();

            _logger.Info("\tStart get game data for {0} accounts in parallel", countAccounts);
            try
            {
                _performanceApi.ParallelTask(0, countAccounts, ParallelismFactor, i => _performanceApi.GetGameData(clientLogins[i]), counter =>
                {
                    if (counter % ParallelismFactor == 0)
                    {
                        Console.WriteLine("Got game data for {0} accounts", counter);
                    }
                });
                /*for (int i = 0; i < countAccounts; i++)
                {
                    var result = GetGameData(clientLogins[i]).Result;
                    if (i % ParallelismFactor == 0)
                    {
                        Console.WriteLine("Got game data for {0} accounts", i);
                    }
                }*/
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            _logger.Info("\tDone get game data for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
        }


        public void TestGetGameData(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = _performanceApi.LoginAsAdmin().Result;

            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            ClientConnectionInfo[] clientLogins = _performanceApi.LoginManyClients(countAccounts, accounLoginPrefix);
            Thread.Sleep(1000);

            if (useParallel)
            {
                _logger.Info("\tStart get game data for {0} accounts in parallel", countAccounts);
                sw.Start();
                List<Task<GameDataModel>> gemaDataTasks = new List<Task<GameDataModel>>();
                try
                {
                    for (var i = 0; i < countAccounts; ++i)
                    {
                        gemaDataTasks.Add(_performanceApi.GetGameData(clientLogins[i]));
                    };
                    Task.WaitAll(gemaDataTasks.ToArray());
                }
                catch (Exception)
                {

                }
                _logger.Info("\tDone get game data for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
                testResult.Add(new TestResultItem { ActionName = "GetGameData (Parallel)", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            }
            else
            {
                _logger.Info("\tStart get game data for {0} accounts", countAccounts);
                sw.Restart();
                try
                {
                    for (var i = 0; i < countAccounts; ++i)
                    {
                        var result = _performanceApi.GetGameData(clientLogins[i]).Result;
                    };
                }
                catch (Exception)
                {

                }
                _logger.Info("\tDone get game data for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
                testResult.Add(new TestResultItem { ActionName = "GetGameData", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            }
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.LogoutManyClients(clientLogins);
            testResult.Add(new TestResultItem { ActionName = "LogoutManyClients", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, true);
            testResult.Add(new TestResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            _performanceApi.Logout(adminConnection).Wait();
            Thread.Sleep(1000);

            _testsInfo.Add(new TestInfo { TestName = "TestGetGameData", TestResults = testResult });
        }

        public void TestPerformStepAction(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = _performanceApi.LoginAsAdmin().Result;

            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            ClientConnectionInfo[] clientLogins = _performanceApi.LoginManyClients(countAccounts, accounLoginPrefix);
            Thread.Sleep(1000);

            if (useParallel)
            {
                _logger.Info("\tStart perform step for {0} accounts in parallel", countAccounts);
                sw.Restart();
                List<Task<ManualStepResultModel>> gemaDataTasks = new List<Task<ManualStepResultModel>>();
                for (var i = 0; i < countAccounts; ++i)
                {
                    gemaDataTasks.Add(_performanceApi.PerformStep(clientLogins[i]));
                };
                Task.WaitAll(gemaDataTasks.ToArray());
                _logger.Info("\tDone perform step for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
                testResult.Add(new TestResultItem { ActionName = "PerformSte (Parallel)", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            }
            else
            {
                _logger.Info("\tStart perform step for {0} accounts", countAccounts);
                sw.Restart();
                for (var i = 0; i < countAccounts; ++i)
                {
                    var result = _performanceApi.GetGameData(clientLogins[i]).Result;
                };

                _logger.Info("\tDone perform step for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
                testResult.Add(new TestResultItem { ActionName = "PerformStep", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            }
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.LogoutManyClients(clientLogins);
            testResult.Add(new TestResultItem { ActionName = "LogoutManyClients", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, true);
            testResult.Add(new TestResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            _performanceApi.Logout(adminConnection).Wait();
            Thread.Sleep(1000);
            _testsInfo.Add(new TestInfo { TestName = "TestPerformStepAction", TestResults = testResult });
        }

        public void TestPerformanceRegisterManyAccounts(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = _performanceApi.LoginAsAdmin().Result;
            Thread.Sleep(1000);

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Start();
            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            testResult.Add(new TestResultItem { ActionName = "RegisterManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            sw.Restart();
            _logger.Info("\tStart delete {0} accounts", countAccounts);
            var deleteResult = _performanceApi.DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tSuccess deleted {0} accounts, Not found: {1}", deleteResult.Item1, deleteResult.Item2);
            testResult.Add(new TestResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            _performanceApi.Logout(adminConnection).Wait();
            Thread.Sleep(1000);
            _testsInfo.Add(new TestInfo { TestName = "TestPerformanceRegisterManyAccounts", TestResults = testResult });
        }
    }
}
