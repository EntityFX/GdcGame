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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFx.Gdcame.Test.PerfomanceFramework;
using Newtonsoft.Json;
using PerformanceCounter = EntityFx.Gdcame.Test.PerfomanceFramework.PerformanceCounter;

namespace EntityFx.Gdcame.Test.Perfomance
{
    class Program
    {


        static void Main(string[] args)
        {
            Uri[] serviceAddressList;
            var countRequests = 10000;
            var port = 80;
            if (args.Length > 0)
            {
                port = Convert.ToInt32(args[0]);
            }
            else
            {
                serviceAddressList = GetServers().Select(_ => new Uri("http://" + _)).ToArray();
            }

            if (args.Length > 2)
            {
                serviceAddressList = new[]
                {
                    new Uri(args[2])
                };
            }
            else
            {
                serviceAddressList = GetServers().Select(_ => new Uri("http://"+ _ + ":" + port)).ToArray();
            }

            if (args.Length > 1)
            {
                countRequests = Convert.ToInt32(args[1]);
            }

            var logger = new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")));

            var performanceTester = new PerfomanceTester(serviceAddressList, logger);

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

        public static string[] GetServers()
        {
            if (!File.Exists("../servers.json"))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText("../servers.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (string[])serializer.Deserialize(file, typeof(string[]));
            }
        }
    }



    class PerfomanceTester
    {
        public const int ParallelismFactor = 256;

        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string DefaultPassword = "P@ssw0rd";

        private readonly Uri[] _serviceUri;

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



        public PerfomanceTester(Uri[] serviceUriList, ILogger logger)
        {
            _serviceUri = serviceUriList;
            _logger = logger;
            _performanceApi = new PerformanceApi(serviceUriList, logger);
        }

        public void TestStartManyGames(int countAccounts, string accounLoginPrefix)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = _performanceApi.LoginAsAdmin().Result.Item2;

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Restart();
            var registerManyAccountsResult = _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, true);
            PrintPerfomanceAggregateResults(registerManyAccountsResult);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);

            _logger.Info("\tStart login {0} accounts in parallel", countAccounts);
            var loginManyAccountsResult = _performanceApi.LoginManyClients(countAccounts, accounLoginPrefix);
            PrintPerfomanceAggregateResults(loginManyAccountsResult.Item1);
            _logger.Info("\tDone login {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            sw.Restart();

            _logger.Info("\tStart get game data for {0} accounts in parallel", countAccounts);
            Tuple<PerformanceAggregate, GameDataModel[]> getGameDataResult = null;
            try
            {
                getGameDataResult = _performanceApi.DoParallelTask(0, countAccounts, ParallelismFactor, i => _performanceApi.GetGameData(loginManyAccountsResult.Item2[i]), counter =>
                {
                    if (counter % ParallelismFactor == 0)
                    {
                        Console.WriteLine("Got game data for {0} accounts", counter);
                    }
                });
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            if (getGameDataResult != null) PrintPerfomanceAggregateResults(getGameDataResult.Item1);
            _logger.Info("\tDone get game data for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
        }

        private void PrintPerfomanceAggregateResults(PerformanceAggregate performanceAggregate)
        {
            Console.WriteLine("Total={0}, Min={1}, Max={2}, Avg={3} ms"
                , performanceAggregate.TotalElapsed, performanceAggregate.Min, performanceAggregate.Max, performanceAggregate.AvgMilliSeconds);
            Console.WriteLine("Statistics by server:");
            foreach (var serverErrorsCount in performanceAggregate.PerformanceStatisticsByServer)
            {
                Console.WriteLine("\t{0}:", serverErrorsCount.Key);
                Console.WriteLine("\t\tTotal={0}, Mi ={1}, Max={2}, Avg={3} ms", 
                    serverErrorsCount.Value.TotalElapsed, serverErrorsCount.Value.Min, serverErrorsCount.Value.Max, serverErrorsCount.Value.AvgMilliSeconds);
                Console.WriteLine("\t\t Errors: {0}", serverErrorsCount.Value.CountErrors);
            }
        }

        public void TestGetGameData(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = _performanceApi.LoginAsAdmin().Result.Item2;

            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            ClientConnectionInfo[] clientLogins = _performanceApi.LoginManyClients(countAccounts, accounLoginPrefix).Item2;
            Thread.Sleep(1000);

            if (useParallel)
            {
                _logger.Info("\tStart get game data for {0} accounts in parallel", countAccounts);
                sw.Start();
                List<Task<Tuple<PerformanceCounter, GameDataModel>>> gemaDataTasks = new List<Task<Tuple<PerformanceCounter, GameDataModel>>>();
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
            var adminConnection = _performanceApi.LoginAsAdmin().Result.Item2;

            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            ClientConnectionInfo[] clientLogins = _performanceApi.LoginManyClients(countAccounts, accounLoginPrefix).Item2;
            Thread.Sleep(1000);

            if (useParallel)
            {
                _logger.Info("\tStart perform step for {0} accounts in parallel", countAccounts);
                sw.Restart();
                List<Task<Tuple<PerformanceCounter, ManualStepResultModel>>> gemaDataTasks = new List<Task<Tuple<PerformanceCounter, ManualStepResultModel>>>();
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
            var adminConnection = _performanceApi.LoginAsAdmin().Result.Item2;
            Thread.Sleep(1000);

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Start();
            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            testResult.Add(new TestResultItem { ActionName = "RegisterManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            sw.Restart();
            _logger.Info("\tStart delete {0} accounts", countAccounts);
            var deleteResult = _performanceApi.DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tSuccess deleted {0} accounts, Not found: {1}", countAccounts, 0);
            testResult.Add(new TestResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            _performanceApi.Logout(adminConnection).Wait();
            Thread.Sleep(1000);
            _testsInfo.Add(new TestInfo { TestName = "TestPerformanceRegisterManyAccounts", TestResults = testResult });
        }
    }
}
