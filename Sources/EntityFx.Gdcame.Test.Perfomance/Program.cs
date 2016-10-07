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

namespace EntityFx.Gdcame.Test.Perfomance
{
    class Program
    {


        static void Main(string[] args)
        {
            var serviceAddress = "http://localhost:9001/";
            if (args.Length > 0)
            {
                serviceAddress = args[0];
            }

            var logger = new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")));

            var performanceTester = new PerfomanceTester(new Uri(serviceAddress), logger);

            performanceTester.TestStartManyGames(40000, RandomString(15));
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

    class ClientConnectionInfo
    {
        public string Login { get; set; }

        public PasswordOAuthContext Context { get; set; }
    }

    class TestResultItem
    {
        public string ActionName { get; set; }

        public TimeSpan TotalElapsed { get; set; }

        public double ElapsedMilliseconds { get; set; }
    }

    class TestInfo
    {
        public string TestName { get; set; }

        public List<TestResultItem> TestResults { get; set; }
    }

    class PerfomanceTester
    {
        public const int ParallelismFactor = 256;

        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string DefaultPassword = "P@ssw0rd";

        private readonly Uri _serviceUri;

        private readonly ILogger _logger;

        private readonly List<TestInfo> _testsInfo = new List<TestInfo>();

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
        }

        public void TestStartManyGames(int countAccounts, string accounLoginPrefix)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = LoginAsAdmin().Result;

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Restart();
            RegisterManyAccounts(countAccounts, accounLoginPrefix, true);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);

            _logger.Info("\tStart login {0} accounts in parallel", countAccounts);
            ClientConnectionInfo[] clientLogins = LoginManyClients(countAccounts, accounLoginPrefix);
            _logger.Info("\tDone login {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            sw.Restart();

            _logger.Info("\tStart get game data for {0} accounts in parallel", countAccounts);
            try
            {
                ParallelTask(0, countAccounts, ParallelismFactor, i => GetGameData(clientLogins[i]), counter =>
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
            var adminConnection = LoginAsAdmin().Result;

            RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            ClientConnectionInfo[] clientLogins = LoginManyClients(countAccounts, accounLoginPrefix);
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
                        gemaDataTasks.Add(GetGameData(clientLogins[i]));
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
                        var result = GetGameData(clientLogins[i]).Result;
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
            LogoutManyClients(clientLogins);
            testResult.Add(new TestResultItem { ActionName = "LogoutManyClients", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            sw.Restart();
            DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, true);
            testResult.Add(new TestResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            Logout(adminConnection).Wait();
            Thread.Sleep(1000);

            _testsInfo.Add(new TestInfo { TestName = "TestGetGameData", TestResults = testResult });
        }

        public void TestPerformStepAction(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = LoginAsAdmin().Result;

            RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            ClientConnectionInfo[] clientLogins = LoginManyClients(countAccounts, accounLoginPrefix);
            Thread.Sleep(1000);

            if (useParallel)
            {
                _logger.Info("\tStart perform step for {0} accounts in parallel", countAccounts);
                sw.Restart();
                List<Task<ManualStepResultModel>> gemaDataTasks = new List<Task<ManualStepResultModel>>();
                for (var i = 0; i < countAccounts; ++i)
                {
                    gemaDataTasks.Add(PerformStep(clientLogins[i]));
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
                    var result = GetGameData(clientLogins[i]).Result;
                };

                _logger.Info("\tDone perform step for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
                testResult.Add(new TestResultItem { ActionName = "PerformStep", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            }
            Thread.Sleep(1000);

            sw.Restart();
            LogoutManyClients(clientLogins);
            testResult.Add(new TestResultItem { ActionName = "LogoutManyClients", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            sw.Restart();
            DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, true);
            testResult.Add(new TestResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });
            Thread.Sleep(1000);

            Logout(adminConnection).Wait();
            Thread.Sleep(1000);
            _testsInfo.Add(new TestInfo { TestName = "TestPerformStepAction", TestResults = testResult });
        }

        public void TestPerformanceRegisterManyAccounts(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var testResult = new List<TestResultItem>();
            var sw = new Stopwatch();
            var adminConnection = LoginAsAdmin().Result;
            Thread.Sleep(1000);

            _logger.Info("\tStart register {0} accounts in parallel", countAccounts);
            sw.Start();
            RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tDone register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            testResult.Add(new TestResultItem { ActionName = "RegisterManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            sw.Restart();
            _logger.Info("\tStart delete {0} accounts", countAccounts);
            var deleteResult = DeleteManyAccounts(adminConnection, countAccounts, accounLoginPrefix, useParallel);
            _logger.Info("\tSuccess deleted {0} accounts, Not found: {1}", deleteResult.Item1, deleteResult.Item2);
            testResult.Add(new TestResultItem { ActionName = "DeleteManyAccounts", ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds / countAccounts, TotalElapsed = sw.Elapsed });

            Logout(adminConnection).Wait();
            Thread.Sleep(1000);
            _testsInfo.Add(new TestInfo { TestName = "TestPerformanceRegisterManyAccounts", TestResults = testResult });
        }

        private void ParallelTask(int from, int to, int parallelism, Func<int, Task> task, Action<int> iterrationAction)
        {
            var counter = 0;
            object stdLock = new { };
            for (var i = from; i < to; i += parallelism)
            {
                List<Task> registerTasks = new List<Task>();
                for (int i1 = i; i1 < ((i + parallelism) > to ? to : i + parallelism); i1++)
                {
                    registerTasks.Add(task(i1));
                }
                Task.WaitAll(registerTasks.ToArray());
                lock (stdLock)
                {
                    iterrationAction(counter);
                    counter += parallelism;
                }
            };
        }

        private void RegisterManyAccounts(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            if (useParallel)
            {
                ParallelTask(0, countAccounts, ParallelismFactor, i1 => RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i1)), counter =>
                {
                    if (counter % ParallelismFactor == 0)
                    {
                        Console.WriteLine("Registered {0} accounts", counter);
                    }
                });
                /*for (var i = 0; i < countAccounts; ++i)
                {
                    registerTasks.Add(RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i)));
                };
                Task.WhenAll(registerTasks.ToArray()).Wait();*/
            }
            else
            {
                for (var i = 0; i < countAccounts; ++i)
                {
                    var res = RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i)).Result;
                };
            }
        }

        private Tuple<int, int> DeleteManyAccounts(ClientConnectionInfo adminConnectionInfo, int countAccounts, string accounLoginPrefix, bool useParallel)
        {

            int successDeleted = 0;
            int notFound = 0;

            ParallelTask(0, countAccounts, ParallelismFactor, i1 => Task.Run(() => FindAndDeleteAccount(adminConnectionInfo, string.Format("{0}{1}", accounLoginPrefix, i1))), counter =>
            {
                if (counter % ParallelismFactor == 0)
                {
                    Console.WriteLine("Deleted {0} accounts", counter);
                }
            });
            /*for (var i = 0; i < countAccounts; ++i)
            {
                var deleteResult = FindAndDeleteAccount(adminConnectionInfo, string.Format("{0}{1}", accounLoginPrefix, i));
                if (deleteResult)
                {
                    successDeleted++;
                }
                else
                {
                    notFound++;
                }
            };*/
            return new Tuple<int, int>(successDeleted, notFound);
        }

        private ClientConnectionInfo[] LoginManyClients(int countAccounts, string accounLoginPrefix)
        {
            //List<Task<ClientConnectionInfo>> loginTasks = new List<Task<ClientConnectionInfo>>();
            ClientConnectionInfo[] logins = new ClientConnectionInfo[countAccounts];

            ParallelTask(0, countAccounts, ParallelismFactor, async i1 =>
            {
                logins[i1] = await Login(string.Format("{0}{1}", accounLoginPrefix, i1), DefaultPassword);

            }, counter =>
            {
                if (counter % ParallelismFactor == 0)
                {
                    Console.WriteLine("Logged in {0} accounts", counter);
                }
            });

            /*for (var i = 0; i < countAccounts; ++i)
            {
                loginTasks.Add(Login(string.Format("{0}{1}", accounLoginPrefix, i), DefaultPassword));
            };
            Task.WaitAll(loginTasks.ToArray());
            return loginTasks.Select(_ => _.Result).ToArray();*/
            return logins;
        }

        private void LogoutManyClients(ClientConnectionInfo[] clients)
        {
            ParallelTask(0, clients.Length, ParallelismFactor, i1 => Logout(clients[i1]), counter =>
            {
                if (counter % ParallelismFactor == 0)
                {
                    Console.WriteLine("Logged out {0} accounts", counter);
                }
            });


            /*foreach (var client in clients)
            {
                logoutTasks.Add(Logout(client));
            }

            Task.WaitAll(logoutTasks.ToArray());*/
        }


        private async Task<ClientConnectionInfo> LoginAsAdmin()
        {
            return await Login("admin", DefaultPassword);
        }

        private async Task<ClientConnectionInfo> Login(string login, string password)
        {
            var p = new PasswordAuthProvider(_serviceUri);
            try
            {
                var token = await p.Login(new PasswordAuthRequest<PasswordAuthData>()
                {
                    RequestData = new PasswordAuthData() { Password = password, Usename = login }
                });
                return new ClientConnectionInfo { Login = login, Context = token };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        private async Task<object> Logout(ClientConnectionInfo client)
        {
            try
            {
                var authApi = new AuthApiClient(client.Context);
                return await authApi.Logout();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        private async Task<object> RegisterAccount(string login)
        {

            var authApi = new AuthApiClient(new PasswordOAuthContext() { BaseUri = _serviceUri });
            try
            {
                return await authApi.Register(new EntityFX.Gdcame.Application.WebApi.Models.RegisterAccountModel()
                {
                    Login = login,
                    Password = DefaultPassword,
                    ConfirmPassword = DefaultPassword
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        private bool FindAndDeleteAccount(ClientConnectionInfo adminConnectionInfo, string login)
        {
            var accountApi = new AccountApiClient(adminConnectionInfo.Context);
            try
            {
                var acount = accountApi.GetByLoginAsync(login).Result;
                var delResult = accountApi.DeleteAsync(acount.UserId).Result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return true;
        }

        private async Task<GameDataModel> GetGameData(ClientConnectionInfo client)
        {
            try
            {
                var gameClient = new GameApiClient(client.Context);
                return await gameClient.GetGameDataAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        private async Task<ManualStepResultModel> PerformStep(ClientConnectionInfo client)
        {
            try
            {
                var gameClient = new GameApiClient(client.Context);
                return await gameClient.PerformManualStepAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }
    }
}
