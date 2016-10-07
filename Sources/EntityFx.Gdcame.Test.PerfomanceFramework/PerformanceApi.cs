using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class PerformanceApi
    {
        public const int ParallelismFactor = 256;

        private ClientConnectionInfo[] ClientConnections { get; set; }

        private const string DefaultPassword = "P@ssw0rd";

        private readonly Uri _serviceUri;

        private readonly ILogger _logger;

        private readonly List<TestInfo> _testsInfo = new List<TestInfo>();


        public PerformanceApi(Uri serviceUri, ILogger logger)
        {
            _serviceUri = serviceUri;
            _logger = logger;
        }

        public void ParallelTask(int from, int to, int parallelism, Func<int, Task> task, Action<int> iterrationAction)
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

        public void RegisterManyAccounts(int countAccounts, string accounLoginPrefix, bool useParallel)
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

        public Tuple<int, int> DeleteManyAccounts(ClientConnectionInfo adminConnectionInfo, int countAccounts, string accounLoginPrefix, bool useParallel)
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

        public ClientConnectionInfo[] LoginManyClients(int countAccounts, string accounLoginPrefix)
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

        public void LogoutManyClients(ClientConnectionInfo[] clients)
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


        public async Task<ClientConnectionInfo> LoginAsAdmin()
        {
            return await Login("admin", DefaultPassword);
        }

        public async Task<ClientConnectionInfo> Login(string login, string password)
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

        public async Task<object> Logout(ClientConnectionInfo client)
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

        public async Task<object> RegisterAccount(string login)
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

        public bool FindAndDeleteAccount(ClientConnectionInfo adminConnectionInfo, string login)
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

        public async Task<GameDataModel> GetGameData(ClientConnectionInfo client)
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

        public async Task<ManualStepResultModel> PerformStep(ClientConnectionInfo client)
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
