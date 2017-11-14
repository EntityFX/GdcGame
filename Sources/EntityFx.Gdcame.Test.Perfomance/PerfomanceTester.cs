using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EntityFx.Gdcame.Test.PerfomanceFramework;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using PerformanceCounter = EntityFx.Gdcame.Test.PerfomanceFramework.PerformanceCounter;

namespace EntityFx.Gdcame.Test.Perfomance
{
    class PerfomanceTester
    {
        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string DefaultPassword = "P@ssw0rd";

        private readonly Uri[] _serviceUri;

        private readonly ILogger _logger;
        private readonly int _parallelism;

        private readonly List<TestResultInfo> _testsInfo = new List<TestResultInfo>();

        private readonly PerformanceApi _performanceApi;

        public IReadOnlyList<TestResultInfo> TestInfo
        {
            get { return _testsInfo; }
        }



        public PerfomanceTester(Uri[] serviceUriList, ILogger logger, int parallelism)
        {
            _serviceUri = serviceUriList;
            _logger = logger;
            _parallelism = parallelism;
            _performanceApi = new PerformanceApi(serviceUriList, logger);
        }

        public Tuple<PerformanceAggregate, string[]> TestEchoAuth(int countAccounts, string text, bool isParallel)
        {
            var sw = new Stopwatch();
            Tuple<PerformanceAggregate, string[]> getGameDataResult = null;

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Restart();
            var registerManyAccountsResult = _performanceApi.RegisterManyAccounts(countAccounts, text, isParallel);
            PrintPerfomanceAggregateResults(registerManyAccountsResult);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts,
                sw.Elapsed, sw.Elapsed.TotalMilliseconds/countAccounts);

            _logger.Info("\tStart login {0} accounts in parallel", countAccounts);
            var loginManyAccountsResult = _performanceApi.LoginManyClients(countAccounts, text);
            PrintPerfomanceAggregateResults(loginManyAccountsResult.Item1);
            _logger.Info("\tDone login {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed,
                sw.Elapsed.TotalMilliseconds/countAccounts);


            _logger.Info("\tStart echo with auth {0} in parallel", countAccounts);
            sw.Restart();
            if (isParallel)
            {
                try
                {
                    getGameDataResult = _performanceApi.DoParallelTasks(0, countAccounts, _parallelism,
                        i => _performanceApi.Echo(loginManyAccountsResult.Item2[i], text)
                        , counter =>
                        {
                            if (counter%_parallelism == 0)
                            {
                                Console.WriteLine("Echoed {0} times", counter);
                            }
                        });
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }
            else
            {
                try
                {
                    getGameDataResult = _performanceApi.DoSequenceTask(0, countAccounts,
                        i => _performanceApi.Echo(loginManyAccountsResult.Item2[i], text)
                        , counter =>
                        {
                            if (counter%_parallelism == 0)
                            {
                                Console.WriteLine("Echoed {0} times", counter);
                            }
                        });
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }
            if (getGameDataResult != null) PrintPerfomanceAggregateResults(getGameDataResult.Item1);
            _logger.Info("\tDone echo for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts,
                sw.Elapsed, sw.Elapsed.TotalMilliseconds/countAccounts);

            var adminConnection = _performanceApi.LoginAsAdmin().Result.Item2;
            _performanceApi.StopAllGames(adminConnection);

            return getGameDataResult;
        }

        public Tuple<PerformanceAggregate, string[]> TestEcho(int countAccounts, string text, bool isParallel)
        {
            var sw = new Stopwatch();
            Tuple<PerformanceAggregate, string[]> getGameDataResult = null;
            _logger.Info("\tStart echo  {0} in parallel={1}", countAccounts, isParallel);

            var serverConnections = _performanceApi.GetServerConnections(countAccounts);

            sw.Restart();

            if (isParallel)
            {
                try
                {
                    getGameDataResult = _performanceApi.DoParallelTasks(0, countAccounts, _parallelism,
                        i => _performanceApi.Echo(serverConnections[i], text), counter =>
                        {
                            if (counter% _parallelism == 0)
                            {
                                Console.WriteLine("Echoed {0} times", counter);
                            }
                        });
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }
            else
            {
                try
                {
                    getGameDataResult = _performanceApi.DoSequenceTask(0, countAccounts,
                        i => _performanceApi.Echo(serverConnections[i], text)
                        , counter =>
                        {
                            if (counter % _parallelism == 0)
                            {
                                Console.WriteLine("Echoed {0} times", counter);
                            }
                        });
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            if (getGameDataResult != null) PrintPerfomanceAggregateResults(getGameDataResult.Item1);
            _logger.Info("\tDone echo for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            return getGameDataResult;
        }

        public Tuple<PerformanceAggregate, GameDataModel[]> TestStartManyGames(int countAccounts, string accounLoginPrefix)
        {
            var testResult = new List<TestActionResultItem>();
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
                getGameDataResult = _performanceApi.DoParallelTasks(0, countAccounts, _parallelism, i =>
                    {
                        return loginManyAccountsResult.Item2[i] != null ? _performanceApi.GetGameData(loginManyAccountsResult.Item2[i]) : null;
                    }
                    , counter =>
                    {
                        if (counter % _parallelism == 0)
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
            return getGameDataResult;
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
            var testResult = new List<TestActionResultItem>();
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
                testResult.Add(new TestActionResultItem { ActionName = "GetGameData (Parallel)", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
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
                testResult.Add(new TestActionResultItem { ActionName = "GetGameData", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            }
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.LogoutManyClients(clientLogins);
            testResult.Add(new TestActionResultItem { ActionName = "LogoutManyClients", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, true);
            testResult.Add(new TestActionResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            _performanceApi.Logout(adminConnection).Wait();
            Thread.Sleep(1000);

            _testsInfo.Add(new TestResultInfo { TestName = "TestGetGameData", TestActionResults = testResult.ToArray() });
        }

        public void TestPerformStepAction(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestActionResultItem>();
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
                testResult.Add(new TestActionResultItem { ActionName = "PerformSte (Parallel)", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
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
                testResult.Add(new TestActionResultItem { ActionName = "PerformStep", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            }
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.LogoutManyClients(clientLogins);
            testResult.Add(new TestActionResultItem { ActionName = "LogoutManyClients", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            sw.Restart();
            _performanceApi.DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, true);
            testResult.Add(new TestActionResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            _performanceApi.Logout(adminConnection).Wait();
            Thread.Sleep(1000);
            _testsInfo.Add(new TestResultInfo { TestName = "TestPerformStepAction", TestActionResults = testResult.ToArray() });
        }

        public void TestPerformanceRegisterManyAccounts(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestActionResultItem>();
            var sw = new Stopwatch();
            var adminConnection = _performanceApi.LoginAsAdmin().Result.Item2;
            Thread.Sleep(1000);

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Start();
            _performanceApi.RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            testResult.Add(new TestActionResultItem { ActionName = "RegisterManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            sw.Restart();
            _logger.Info("\tStart delete {0} accounts", countAccounts);
            var deleteResult = _performanceApi.DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tSuccess deleted {0} accounts, Not found: {1}", countAccounts, 0);
            testResult.Add(new TestActionResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            _performanceApi.Logout(adminConnection).Wait();
            Thread.Sleep(1000);
            _testsInfo.Add(new TestResultInfo { TestName = "TestPerformanceRegisterManyAccounts", TestActionResults = testResult.ToArray() });
        }
    }
}