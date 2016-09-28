using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Presentation.Web.Api.Models;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
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
            var performanceTester = new PerfomanceTester();
            Array.ForEach(new int[] { 10, 50, 100, 500, 1000, 5000, 10000, 50000 }, i =>
            {
                Console.WriteLine("Testing with {0} accounts", i);
                Console.WriteLine("Test: Register accounts");
                performanceTester.RegisterManyAccounts(i, RandomString(10), false);
                Console.WriteLine("Test: Register accounts in parallel");
                performanceTester.RegisterManyAccounts(i, RandomString(10), true);
                Console.WriteLine("Test: Get game data");
                performanceTester.TestGetGameData(i, RandomString(10), false);
                Console.WriteLine("Test: Get game data in parallel");
                performanceTester.TestGetGameData(i, RandomString(10), true);
                Console.WriteLine("Test: Perform step");
                performanceTester.TestPerformStepAction(i, RandomString(10), false);
                Console.WriteLine("Test: Perform step in parallel");
                performanceTester.TestPerformStepAction(i, RandomString(10), true);
                Console.WriteLine("Press any key to close...");
                Console.WriteLine();
            });
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

    class PerfomanceTester
    {
        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string ServiceUri = "http://10.182.171.143:9001/";

        private const string DefaultPassword = "P@ssw0rd";

        public void TestGetGameData(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            Task<ClientConnectionInfo>[] clientLogins;
            List<Task<ClientConnectionInfo>> loginTasks = new List<Task<ClientConnectionInfo>>();
            for (var i = 0; i < countAccounts; ++i)
            {
                loginTasks.Add(Login(string.Format("{0}{1}", accounLoginPrefix, i), DefaultPassword));
            };
            try
            {
                Task.WaitAll(loginTasks.ToArray());
            }
            catch (Exception)
            {

            }
            clientLogins = loginTasks.ToArray();
            Thread.Sleep(1000);

            if (useParallel)
            {
                Console.WriteLine("Start get game data for {0} accounts in parallel", countAccounts);
                var sw = Stopwatch.StartNew();
                List<Task<GameDataModel>> gemaDataTasks = new List<Task<GameDataModel>>();
                try
                {
                    for (var i = 0; i < countAccounts; ++i)
                    {
                        gemaDataTasks.Add(GetGameData(clientLogins[i].Result));
                    };
                    Task.WaitAll(gemaDataTasks.ToArray());
                }
                catch (Exception)
                {

                }
                Console.WriteLine("Done get game data for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            }
            else
            {
                Console.WriteLine("Start get game data for {0} accounts", countAccounts);
                var sw = Stopwatch.StartNew();
                try
                {
                    for (var i = 0; i < countAccounts; ++i)
                    {
                        var result = GetGameData(clientLogins[i].Result).Result;
                    };
                }
                catch (Exception)
                {

                }
                Console.WriteLine("Done get game data for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            }
            Thread.Sleep(1000);

            var adminConnection = LoginAsAdmin().Result;
            foreach (var login in clientLogins)
            {
                try
                {
                    Logout(login.Result).Wait();
                    FindAndDeleteAccount(adminConnection, login.Result.Login);
                }
                catch (Exception)
                {
                }
            }
            Logout(adminConnection).Wait();
        }

        public void TestPerformStepAction(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            RegisterManyAccounts(countAccounts, accounLoginPrefix, useParallel);
            Thread.Sleep(1000);

            ClientConnectionInfo[] clientLogins;
            List<Task<ClientConnectionInfo>> loginTasks = new List<Task<ClientConnectionInfo>>();
            for (var i = 0; i < countAccounts; ++i)
            {
                loginTasks.Add(Login(string.Format("{0}{1}", accounLoginPrefix, i), DefaultPassword));
            };
            try
            {
                Task.WaitAll(loginTasks.ToArray());
            }
            catch (Exception)
            {

            }
            clientLogins = loginTasks.Select(_ => _.Result).ToArray();
            Thread.Sleep(1000);

            if (useParallel)
            {
                Console.WriteLine("Start perform step for {0} accounts in parallel", countAccounts);
                var sw = Stopwatch.StartNew();
                List<Task<ManualStepResultModel>> gemaDataTasks = new List<Task<ManualStepResultModel>>();
                for (var i = 0; i < countAccounts; ++i)
                {
                    gemaDataTasks.Add(PerformStep(clientLogins[i]));
                };
                try
                {
                    Task.WaitAll(gemaDataTasks.ToArray());
                }
                catch (Exception)
                {

                }
                Console.WriteLine("Done perform step for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            }
            else
            {
                Console.WriteLine("Start perform step for {0} accounts", countAccounts);
                var sw = Stopwatch.StartNew();
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
                Console.WriteLine("Don perform step for {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            }
            Thread.Sleep(1000);

            var adminConnection = LoginAsAdmin().Result;
            foreach (var login in clientLogins)
            {
                try
                {
                    Logout(login).Wait();
                    FindAndDeleteAccount(adminConnection, login.Login);
                }
                catch (Exception)
                {
                }
            }
            Logout(adminConnection).Wait();
        }

        public  void RegisterManyAccounts(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            var adminConnection = LoginAsAdmin().Result;
            Console.WriteLine("Start delete {0} accounts", countAccounts);
            int successDeleted = 0;
            int notFound = 0;
            for (var i = 0; i < countAccounts; ++i)
            {
                var deleteResult = FindAndDeleteAccount(adminConnection, string.Format("{0}{1}", accounLoginPrefix, i));
                if (deleteResult)
                {
                    successDeleted++;
                }
                else
                {
                    notFound++;
                }
            };
            Thread.Sleep(1000);
            Console.WriteLine("Success deleted {0} accounts, Not found: {1}", successDeleted, notFound);

            var sw = Stopwatch.StartNew();
            if (useParallel)
            {
                Console.WriteLine("Start register {0} accounts in parallel", countAccounts);
                List<Task> registerTasks = new List<Task>();
                for (var i = 0; i < countAccounts; ++i)
                {
                    registerTasks.Add(RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i)));
                };
                Task.WhenAll(registerTasks.ToArray()).Wait();
                Console.WriteLine("Done register {0} accounts, elapsed: {1}", countAccounts, sw.Elapsed);
            }
            else
            {
                Console.WriteLine("Start register {0} accounts", countAccounts);
                for (var i = 0; i < countAccounts; ++i)
                {
                    var res = RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i)).Result;
                };
                sw.Stop();
                Console.WriteLine("Done register {0} accounts, elapsed: {1}, {2} milliseconds per one", countAccounts, sw.Elapsed, sw.Elapsed.TotalMilliseconds / countAccounts);
            }
            Thread.Sleep(1000);
            Logout(adminConnection).Wait();
        }

        private async Task<ClientConnectionInfo> LoginAsAdmin()
        {
            return await Login("admin", DefaultPassword);
        }

        private async Task<ClientConnectionInfo> Login(string login, string password)
        {
            var p = new PasswordAuthProvider(new Uri(ServiceUri));
            var token = await p.Login(new PasswordAuthRequest<PasswordAuthData>()
            {
                RequestData = new PasswordAuthData() { Password = password, Usename = login }
            });
            return new ClientConnectionInfo { Login = login, Context = token };
        }

        private async Task<object> Logout(ClientConnectionInfo client)
        {
            var authApi = new AuthApiClient(client.Context);
            return await authApi.Logout();
        }

        private async Task<object> RegisterAccount(string login)
        {

            var authApi = new AuthApiClient(new PasswordOAuthContext() { BaseUri = new Uri(ServiceUri) });
            try
            {
                return await authApi.Register(new EntityFX.Gdcame.Presentation.Web.Api.Models.RegisterAccountModel()
                {
                    Login = login,
                    Password = DefaultPassword,
                    ConfirmPassword = DefaultPassword
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
                if (ex.InnerException is ClientException) return false;
                throw;
            }
            return true;
        }

        private async Task<GameDataModel> GetGameData(ClientConnectionInfo client)
        {
            var gameClient = new GameApiClient(client.Context);
            return await gameClient.GetGameDataAsync();
        }

        private async Task<ManualStepResultModel> PerformStep(ClientConnectionInfo client)
        {
            var gameClient = new GameApiClient(client.Context);
            return await gameClient.PerformManualStepAsync();
        }
    }
}
