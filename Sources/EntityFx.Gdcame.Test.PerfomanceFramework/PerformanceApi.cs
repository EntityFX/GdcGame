using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Utils.Common;

namespace EntityFx.Gdcame.Test.PerfomanceFramework
{
    public class ClientConnectionInfo
    {
        public string Login { get; set; }

        public PasswordOAuthContext Context { get; set; }
    }

    public class TestResultItem
    {
        public string ActionName { get; set; }

        public TimeSpan TotalElapsed { get; set; }

        public double ElapsedMilliseconds { get; set; }
    }

    public class TestInfo
    {
        public string TestName { get; set; }

        public List<TestResultItem> TestResults { get; set; }
    }

    public class PerformanceCounter
    {
        public TimeSpan Elapsed { get; set; }

        public string ServerUri { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class PerformanceAggregate
    {
        public PerformanceCounter[] PerformanceCounters { get; set; }

        public TimeSpan TotalElapsed { get; set; }

        public TimeSpan Min {
            get { return PerformanceCounters.Min(_ => _.Elapsed); }
        }

        public TimeSpan Max
        {
            get { return PerformanceCounters.Max(_ => _.Elapsed); }
        }

        public double AvgMilliSeconds
        {
            get { return PerformanceCounters.Average(_ => _.Elapsed.Milliseconds); }
        }

        public int CountErrors
        {
            get { return PerformanceCounters.Count(p => !p.IsSuccess); }
        }

        public IDictionary<string, PerformanceAggregate> PerformanceStatisticsByServer
        {
            get { return PerformanceCounters.GroupBy(_ => _.ServerUri).ToDictionary(
                _ => _.Key, 
                _ => new PerformanceAggregate
                {
                    PerformanceCounters = _.ToArray(),
                    TotalElapsed = TimeSpan.FromMilliseconds(_.Sum(p => p.Elapsed.Milliseconds))
                }); }
        }
    }

    public class PerformanceApi
    {
        public const int ParallelismFactor = 256;

        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string DefaultPassword = "P@ssw0rd";

        private readonly Uri[] _serviceUriList;

        private readonly ILogger _logger;

        private readonly List<TestInfo> _testsInfo = new List<TestInfo>();


        public PerformanceApi(Uri[] serviceUriList, ILogger logger)
        {
            _serviceUriList = serviceUriList;
            _logger = logger;
        }

        public Tuple<PerformanceAggregate, T[]> DoParallelTask<T>(int from, int to, int parallelism, Func<int, Task<Tuple<PerformanceCounter, T>>> task, Action<int> iterrationAction)
        {
            var counter = 0;
            object stdLock = new { };
            var result = new PerformanceAggregate();
            var tasksResultList = new List<Tuple<PerformanceCounter, T>> ();
            var sw = new Stopwatch();
            sw.Start();
            for (var i = from; i < to; i += parallelism)
            {
                List<Task<Tuple<PerformanceCounter,T>>> registerTasks = new List<Task<Tuple<PerformanceCounter, T>>>();
                for (int i1 = i; i1 < ((i + parallelism) > to ? to : i + parallelism); i1++)
                {
                    registerTasks.Add(task(i1));
                }
                Task.WaitAll(registerTasks.ToArray());
                tasksResultList.AddRange(Task.WhenAll(registerTasks).Result);
                lock (stdLock)
                {
                    iterrationAction(counter);
                    counter += parallelism;
                }
            };
            result.TotalElapsed = sw.Elapsed;
            result.PerformanceCounters = tasksResultList.Select(_ => _.Item1).ToArray();
            return Tuple.Create(result, tasksResultList.Select(_ => _.Item2).ToArray());
        }

        private async Task<Tuple<PerformanceCounter, T>> DoPerformanceMeasureAction<T>(Func<Task<T>> action, string sereverUri)
        {
            var sw = new Stopwatch();
            var performanceCounter = new PerformanceCounter();
            sw.Start();
            var result = default(T);
            try
            {
                var task = action();
                result = await task;
                performanceCounter.Elapsed = await task.ContinueWith((_, s) => ((Stopwatch)s).Elapsed, sw);
                performanceCounter.IsSuccess = true;
            }
            catch (Exception ex)
            {
                performanceCounter.IsSuccess = false;
                performanceCounter.ErrorMessage = ex.Message;
                _logger.Error(ex);
            }
            performanceCounter.ServerUri = sereverUri;
            return new Tuple<PerformanceCounter, T>(performanceCounter, result);
        }

        public PerformanceAggregate RegisterManyAccounts(int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            if (useParallel)
            {
                return DoParallelTask(0, countAccounts, ParallelismFactor, async i1 => await RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i1)), counter =>
                {
                    if (counter % ParallelismFactor == 0)
                    {
                        Console.WriteLine("Registered {0} accounts", counter);
                    }
                }).Item1;
            }
            else
            {
                for (var i = 0; i < countAccounts; ++i)
                {
                    var res = RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i)).Result;
                };
                return null;
            }
        }

        public PerformanceAggregate DeleteManyAccounts(ClientConnectionInfo adminConnectionInfo, int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            return DoParallelTask(0, countAccounts, ParallelismFactor, async i1 => await FindAndDeleteAccount(adminConnectionInfo, string.Format("{0}{1}", accounLoginPrefix, i1)), counter =>
            {
                if (counter % ParallelismFactor == 0)
                {
                    Console.WriteLine("Deleted {0} accounts", counter);
                }
            }).Item1;
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
        }

        public Tuple<PerformanceAggregate, ClientConnectionInfo[]> LoginManyClients(int countAccounts, string accounLoginPrefix)
        {
            return DoParallelTask<ClientConnectionInfo>(0, countAccounts, ParallelismFactor, i1 =>
            {
                var loginResult = Login(string.Format("{0}{1}", accounLoginPrefix, i1), DefaultPassword);
                return loginResult;
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
        }

        public PerformanceAggregate LogoutManyClients(ClientConnectionInfo[] clients)
        {
            return DoParallelTask(0, clients.Length, ParallelismFactor, i1 => Logout(clients[i1]), counter =>
            {
                if (counter % ParallelismFactor == 0)
                {
                    Console.WriteLine("Logged out {0} accounts", counter);
                }
            }).Item1;


            /*foreach (var client in clients)
            {
                logoutTasks.Add(Logout(client));
            }

            Task.WaitAll(logoutTasks.ToArray());*/
        }


        public Task<Tuple<PerformanceCounter, ClientConnectionInfo>> LoginAsAdmin()
        {
            return Login("admin", DefaultPassword);
        }

        public Task<Tuple<PerformanceCounter, ClientConnectionInfo>> Login(string login, string password)
        {
            var serverUri = GetApiServerUri(_serviceUriList, login);
            var p = new PasswordAuthProvider(serverUri);
            return DoPerformanceMeasureAction(async () =>
            {
                var token = await p.Login(new PasswordAuthRequest<PasswordAuthData>()
                {
                    RequestData = new PasswordAuthData() {Password = password, Usename = login}
                });
                return new ClientConnectionInfo {Login = login, Context = token};
            }, serverUri.ToString());
        }

        public Task<Tuple<PerformanceCounter, object>> Logout(ClientConnectionInfo client)
        {
            var authApi = new AuthApiClient(client.Context);
            return DoPerformanceMeasureAction(async () => await authApi.Logout(), client.Context.BaseUri.ToString());
        }

        public Task<Tuple<PerformanceCounter, object>> RegisterAccount(string login)
        {
            var serverUri = GetApiServerUri(_serviceUriList, login);
            var authApi = new AuthApiClient(new PasswordOAuthContext() { BaseUri = serverUri });
            return DoPerformanceMeasureAction(async () => await authApi.Register(new EntityFX.Gdcame.Application.WebApi.Models.RegisterAccountModel()
            {
                Login = login,
                Password = DefaultPassword,
                ConfirmPassword = DefaultPassword
            }), serverUri.ToString());
        }

        public Task<Tuple<PerformanceCounter, bool>> FindAndDeleteAccount(ClientConnectionInfo adminConnectionInfo, string login)
        {
            var accountApi = new AccountApiClient(adminConnectionInfo.Context);
            return DoPerformanceMeasureAction(async () =>
            {
                var acount = accountApi.GetByLoginAsync(login).Result;
                var delResult = await accountApi.DeleteAsync(acount.UserId);
                return delResult;
            }, adminConnectionInfo.Context.BaseUri.ToString());
        }

        public Task<Tuple<PerformanceCounter, GameDataModel>> GetGameData(ClientConnectionInfo client)
        {
            var gameClient = new GameApiClient(client.Context);
            return DoPerformanceMeasureAction(async () => await gameClient.GetGameDataAsync(), client.Context.BaseUri.ToString());
        }

        public Task<Tuple<PerformanceCounter, ManualStepResultModel>> PerformStep(ClientConnectionInfo client)
        {
            var gameClient = new GameApiClient(client.Context);
            return DoPerformanceMeasureAction(async () => await gameClient.PerformManualStepAsync(), client.Context.BaseUri.ToString());
        }

        private static Uri GetApiServerUri(Uri[] serversUriList, string login)
        {
            var hasher = new HashHelper();
            var serverNumber = hasher.GetModuloOfUserIdHash(hasher.GetHashedString(login), serversUriList.Length);
            return serversUriList[serverNumber];
        }
    }
}
