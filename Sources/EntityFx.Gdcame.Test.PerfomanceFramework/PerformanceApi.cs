using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.WebApiClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Utils.Shared;

namespace EntityFx.Gdcame.Test.PerfomanceFramework
{
    public static class ParallelTasks
    {
        /// <summary>
        /// Starts the given tasks and waits for them to complete. This will run, at most, the specified number of tasks in parallel.
        /// <para>NOTE: If one of the given tasks has already been started, an exception will be thrown.</para>
        /// </summary>
        /// <param name="tasksToRun">The tasks to run.</param>
        /// <param name="maxTasksToRunInParallel">The maximum number of tasks to run in parallel.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static async Task StartAndWaitAllThrottledAsync(IEnumerable<Task> tasksToRun, int maxTasksToRunInParallel, CancellationToken cancellationToken = new CancellationToken())
        {
            await StartAndWaitAllThrottledAsync(tasksToRun, maxTasksToRunInParallel, -1, cancellationToken);
        }

        /// <summary>
        /// Starts the given tasks and waits for them to complete. This will run the specified number of tasks in parallel.
        /// <para>NOTE: If a timeout is reached before the Task completes, another Task may be started, potentially running more than the specified maximum allowed.</para>
        /// <para>NOTE: If one of the given tasks has already been started, an exception will be thrown.</para>
        /// </summary>
        /// <param name="tasksToRun">The tasks to run.</param>
        /// <param name="maxTasksToRunInParallel">The maximum number of tasks to run in parallel.</param>
        /// <param name="timeoutInMilliseconds">The maximum milliseconds we should allow the max tasks to run in parallel before allowing another task to start. Specify -1 to wait indefinitely.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static async Task StartAndWaitAllThrottledAsync(IEnumerable<Task> tasksToRun, int maxTasksToRunInParallel, int timeoutInMilliseconds, CancellationToken cancellationToken = new CancellationToken())
        {
            // Convert to a list of tasks so that we don't enumerate over it multiple times needlessly.
            var tasks = tasksToRun.ToList();

            using (var throttler = new SemaphoreSlim(maxTasksToRunInParallel))
            {
                var postTaskTasks = new List<Task>();

                // Have each task notify the throttler when it completes so that it decrements the number of tasks currently running.
                tasks.ForEach(t => postTaskTasks.Add(t.ContinueWith(tsk => throttler.Release())));

                // Start running each task.
                foreach (var task in tasks)
                {
                    // Increment the number of tasks currently running and wait if too many are running.
                    await throttler.WaitAsync(timeoutInMilliseconds, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();
                    //task.Start();
                }

                // Wait for all of the provided tasks to complete.
                // We wait on the list of "post" tasks instead of the original tasks, otherwise there is a potential race condition where the throttler's using block is exited before some Tasks have had their "post" action completed, which references the throttler, resulting in an exception due to accessing a disposed object.
                await Task.WhenAll(postTaskTasks.ToArray());
            }
        }
    }


    public class ClientConnectionInfo
    {
        public string Login { get; set; }

        public PasswordOAuthContext Context { get; set; }
    }

    public class TestActionResultItem
    {
        public TimeSpan TotalElapsed { get; set; }

        public string ActionName { get; set; }

        public string ActionDescription { get; set; }

        public double ElapsedMilliseconds { get; set; }

        public TestStatistics TestStatistics { get; set; }

        public PerformanceCounter[] PerformanceCounters { get; set; }

        public IDictionary<string, TestStatistics> PerformanceStatisticsByServer { get; set; }
    }

    public class TestStatistics
    {

        public TimeSpan Elapsed { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
        public int CountErrors { get; set; }
        public double AvgMilliseconds { get; set; }
    }

    public class TestResultInfo
    {
        public string TestName { get; set; }

        public TestActionResultItem[] TestActionResults { get; set; }
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

        public TimeSpan Min
        {
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
            get
            {
                return PerformanceCounters.GroupBy(_ => _.ServerUri).ToDictionary(
              _ => _.Key,
              _ => new PerformanceAggregate
              {
                  PerformanceCounters = _.ToArray(),
                  TotalElapsed = TimeSpan.FromMilliseconds(_.Sum(p => p.Elapsed.Milliseconds))
              });
            }
        }
    }

    public class PerformanceApi
    {
        private const int ParallelismFactor = 256;

        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string DefaultPassword = "P@ssw0rd";

        private readonly Uri[] _serviceUriList;

        private readonly ILogger _logger;

        private readonly List<TestResultInfo> _testsInfo = new List<TestResultInfo>();


        public PerformanceApi(Uri[] serviceUriList, ILogger logger)
        {
            _serviceUriList = serviceUriList;
            _logger = logger;
        }

        public Tuple<PerformanceAggregate, T[]> DoParallelTasks<T>(int from, int to, int parallelism, Func<int, Task<Tuple<PerformanceCounter, T>>> task, Action<int> iterrationAction)
        {
            var counter = 0;
            var result = new PerformanceAggregate();
            var tasksResultList = new List<Tuple<PerformanceCounter, T>>();
            var sw = new Stopwatch();
            ThreadPool.SetMaxThreads(parallelism*2, parallelism*2);
            sw.Start();
            /*Parallel.For(from, to, (i) =>
            {
                tasksResultList.Add(task(i).Result);
                iterrationAction(counter);
                Interlocked.Increment(ref counter);
            });*/
            for (var i = from; i < to; i += parallelism)
            {
                var tasks = new Task[(i + parallelism) > to ? to - i : parallelism];
                for (int pi = 0; pi < tasks.Length; pi++)
                {
                    tasks[pi] = task(i + pi);
                }
                //ParallelTasks.StartAndWaitAllThrottledAsync(registerTasks, 1000).Wait();
                Task.WaitAll(tasks);
                tasksResultList.AddRange(tasks.Cast<Task<Tuple<PerformanceCounter, T>>>().Select(_ => _.Result));
                iterrationAction(counter);
                Interlocked.Add(ref counter, parallelism);
            };
            result.TotalElapsed = sw.Elapsed;
            result.PerformanceCounters = tasksResultList.Select(_ => _.Item1).ToArray();
            return Tuple.Create(result, tasksResultList.Select(_ => _.Item2).ToArray());
        }

        public Tuple<PerformanceAggregate, T[]> DoSequenceTask<T>(int from, int to, Func<int, Task<Tuple<PerformanceCounter, T>>> task, Action<int> iterrationAction)
        {
            var result = new PerformanceAggregate();
            var tasksResultList = new List<Tuple<PerformanceCounter, T>>();
            var sw = new Stopwatch();
            sw.Start();
            for (var i = from; i < to; i++)
            {
                tasksResultList.Add(task(i).Result);
                if (i % 256 == 0)
                {
                    iterrationAction(i);
                }
            }
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
            Task<T> task;
            try
            {
                task = action();
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
                return DoParallelTasks(0, countAccounts, ParallelismFactor, async i1 => await RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i1)), counter =>
               {
                   if (counter % ParallelismFactor == 0)
                   {
                       Console.WriteLine("Registered {0} accounts", counter);
                   }
               }).Item1;
            }
            else
            {
                return DoSequenceTask(0, countAccounts,
                    i1 => RegisterAccount(string.Format("{0}{1}", accounLoginPrefix, i1)), i => {
                        if (i % ParallelismFactor == 0)
                        {
                            Console.WriteLine("Registered {0} accounts", i);
                        }
                    }).Item1;
            }
        }

        public PerformanceAggregate DeleteManyAccounts(ClientConnectionInfo adminConnectionInfo, int countAccounts, string accounLoginPrefix, bool useParallel)
        {
            return DoParallelTasks(0, countAccounts, ParallelismFactor, async i1 => await FindAndDeleteAccount(adminConnectionInfo, string.Format("{0}{1}", accounLoginPrefix, i1)), counter =>
            {
                if (counter % ParallelismFactor == 0)
                {
                    Console.WriteLine("Deleted {0} accounts", counter);
                }
            }).Item1;
        }

        public void StopAllGames(ClientConnectionInfo adminConnectionInfo)
        {
            Task.WhenAll(DoAuthServersWithAdmin(_serviceUriList, "admin").Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        new AdminApiClient(_).StopAllGames();
                    }
                    catch (Exception)
                    {

                    }
                }))).Wait();
        }

        private PasswordOAuthContext[] DoAuthServersWithAdmin(Uri[] serversList, string adminLogin, string password = DefaultPassword)
        {
            return Task.WhenAll(

                    serversList.Select(
                            server =>
                                new PasswordAuthProvider(
                                    server))
                        .Select(async passwordProvider =>
                        {
                            try
                            {
                                return await passwordProvider.Login(new PasswordAuthRequest<PasswordAuthData>
                                {
                                    RequestData = new PasswordAuthData { Password = password, Usename = adminLogin }
                                });
                            }
                            catch (Exception)
                            {
                                return null;
                                ;
                            }

                        }
                        )).Result;
        }

        public Tuple<PerformanceAggregate, ClientConnectionInfo[]> LoginManyClients(int countAccounts, string accounLoginPrefix)
        {
            return DoParallelTasks<ClientConnectionInfo>(0, countAccounts, ParallelismFactor, i1 =>
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
            return DoParallelTasks(0, clients.Length, ParallelismFactor, i1 => Logout(clients[i1]), counter =>
            {
                if (counter % ParallelismFactor == 0)
                {
                    Console.WriteLine("Logged out {0} accounts", counter);
                }
            }).Item1;
        }


        public Task<Tuple<PerformanceCounter, ClientConnectionInfo>> LoginAsAdmin()
        {
            return Login("admin", DefaultPassword);
        }

        public Task<Tuple<PerformanceCounter, ClientConnectionInfo>> Login(string login, string password)
        {
            var serverUri = GetApiServerUri(_serviceUriList, login);
            var p = new PasswordAuthProvider(serverUri, _logger);
            return DoPerformanceMeasureAction(async () =>
            {
                var token = await p.Login(new PasswordAuthRequest<PasswordAuthData>()
                {
                    RequestData = new PasswordAuthData() { Password = password, Usename = login }
                });
                return new ClientConnectionInfo { Login = login, Context = token };
            }, serverUri.ToString());
        }

        public ClientConnectionInfo[] GetServerConnections(int countRequests)
        {
            var result = new ClientConnectionInfo[countRequests];
            for (int r = 0, s = 0; r < countRequests; r++)
            {
                result[r] = GetConnection(s);
                s = s == _serviceUriList.Length - 1 ? 0 : s + 1;
            }
            return result;
        }

        public ClientConnectionInfo GetConnection(int serverNumber)
        {
            var serverUri = _serviceUriList[serverNumber];
            return new ClientConnectionInfo { Context = new PasswordOAuthContext() { BaseUri = serverUri } };
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
            return DoPerformanceMeasureAction(async () => await authApi.Register(new RegisterAccountModel()
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

        public async Task<Tuple<PerformanceCounter, string>> Echo(ClientConnectionInfo client, string text)
        {
            Random r = new Random(54657);
            return await DoPerformanceMeasureAction(() =>
            {
                var gameClient = new ServerInfoClient(client.Context);
                //return Task.Delay(r.Next(5,500)).ContinueWith<string>(_ =>  "ok");
                return Task.Run(() =>
                {
                    return gameClient.Echo(text);
                });
            }, client.Context.BaseUri.ToString());
        }

        public string Echo2(ClientConnectionInfo client, string text)
        {
            var gameClient = new ServerInfoClient(client.Context);
            return gameClient.Echo(text);
        }

        public Task<string> Echo1(ClientConnectionInfo client, string text)
        {
            /* var gameClient = new ServerInfoClient(client.Context);
             return Task.Run(() =>
             {
                 var performanceCounter = new PerformanceCounter()
                 {
                     ServerUri = client.Context.BaseUri.ToString(), ErrorMessage = null
                 };
                 var res = gameClient.Echo(text);
                 //Thread.Sleep(500);
                 return new Tuple<PerformanceCounter, string>(performanceCounter, "dfgd");
             });*/
            return Task.Run(() =>
            {
                var gameClient = new ServerInfoClient(client.Context);
                return gameClient.Echo(text);
            })

        ;
        }

        private static Uri GetApiServerUri(Uri[] serversUriList, string login)
        {
            var hasher = new HashHelper();
            //TODO: Use Rendezvous Hashing algorithm.
            var serverNumber = hasher.GetModuloOfUserIdHash(hasher.GetHashedString(login), serversUriList.Length);
            return serversUriList[serverNumber];
        }
    }
}
