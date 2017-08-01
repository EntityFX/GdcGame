namespace EntityFX.Gdcame.Presentation.AdminConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Timers;

    using EntityFx.GdCame.Test.Shared;

    using EntityFX.Gdcame.Application.Contract.Model.MainServer;
    using EntityFX.Gdcame.Common.Application.Model;
    using EntityFX.Gdcame.Infrastructure.Api.Auth;
    using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
    using EntityFX.Gdcame.Presentation.ConsoleClient.Common;

    internal class AdminConsole
    {
        private readonly string _password;
        private readonly Uri _mainServer;

        private readonly Uri ratingServer;

        private readonly int _servicePort;

        private readonly int ratingServerPort;

        private readonly string _user;


        //Dictionary<string, Delegate> _menu;
        private bool _exitFlag;

        private Dictionary<ConsoleKey, MenuItem> _menu;

        private Timer _statisticsUpdateTimer;

        private PasswordOAuthContext[] _serversAuthContextList;
        private ServerAuthContext[] _serversAuthWithTypeContextList;
        private object _stdLock = new { };
        private bool _isMainMenu;

        public AdminConsole(string user, string password, Uri mainServer, Uri ratingServer, int port, int ratingServerPort)

        {
            this._user = user;
            this._password = password;
            this._mainServer = mainServer;
            this.ratingServer = ratingServer;
            this._servicePort = port;
            this.ratingServerPort = ratingServerPort;
            this._statisticsUpdateTimer = new Timer(2500);
            this._statisticsUpdateTimer.Elapsed += this.GetStatisticsCallback;

            this.InitMenu();
        }


        private Guid SessionGuid { get; set; }

        private void InitMenu()
        {
            this._menu = new Dictionary<ConsoleKey, MenuItem>
            {
                {
                    ConsoleKey.F1,
                    new MenuItem {MenuText = "List all active sessions", MenuAction = this.GetActiveSessions}
                },
                {ConsoleKey.F2, new MenuItem {MenuText = "Close by session GUID", MenuAction = this.CloseSessionByGuid}},
                {
                    ConsoleKey.F3,
                    new MenuItem
                    {
                        MenuText = "Close by index in list",
                        MenuAction = this.CloseSessionByUserNameAndPositionOfGuid
                    }
                },
                {
                    ConsoleKey.F4,
                    new MenuItem {MenuText = "Class all user's sessions", MenuAction = this.CloseAllUserSessions}
                },
                {ConsoleKey.F5, new MenuItem {MenuText = "Close all sessions", MenuAction = this.CloseAllSessions}},
                {
                    ConsoleKey.F6,
                    new MenuItem
                    {
                        MenuText = "Close all sessions except current",
                        MenuAction = this.CloseAllSessionsExludeThis
                    }
                },
                {ConsoleKey.F7, new MenuItem {MenuText = "Wipe user game", MenuAction = this.WipeUser}},
                {ConsoleKey.F8, new MenuItem {MenuText = "Get statistics", MenuAction = this.GetStatistics}},
                {ConsoleKey.F9, new MenuItem {MenuText = "Echo", MenuAction = this.Echo}},
                {ConsoleKey.F10, new MenuItem {MenuText = "Update servers list", MenuAction = this.GetNodesList}},
                {ConsoleKey.F11, new MenuItem {MenuText = "Merge server", MenuAction = this.AddServer}},
                {ConsoleKey.Backspace, new MenuItem {MenuText = "Stop all games", MenuAction = this.StopAllGames}},
                {ConsoleKey.Escape, new MenuItem {MenuText = "Exit", MenuAction = this.Exit}}
            };
        }

        private void GetNodesList()
        {

        }

        private void StopAllGames()
        {
            var serversList = ApiHelper.GetServersUri(ApiHelper.GetServers(this._mainServer), this._servicePort);

            Task.WhenAll(this.DoAuthServers(serversList).Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
                () =>
                {
                    try
                    {

                        ApiHelper.GetAdminClient(_).StopAllGames();

                    }
                    catch (Exception)
                    {

                    }
                }))).Wait();
        }

        private void Echo()
        {
            var serversList = ApiHelper.GetServersUri(ApiHelper.GetServers(this._mainServer), this._servicePort);

            var echoResults = Task.WhenAll(
                this.DoAuthServers(serversList).Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
                    () =>
                    {
                        try
                        {
                            var result = new Tuple<Uri, string>(_.BaseUri,
                                ApiHelper.GetServerInfoClient(_).Echo("text"));
                            return result;
                        }
                        catch (Exception)
                        {
                            return new Tuple<Uri, string>(_.BaseUri, null);
                        }
                    }))
            ).Result;
            foreach (var echo in echoResults)
            {
                Console.WriteLine("{0}:{1}", echo.Item1, echo.Item2);
            }
        }

        private class ServerLoginContext
        {
            public Uri Uri { get; set; }
            public string ServerType { get; set; }
        }

        private class ServerAuthContext
        {
            public PasswordOAuthContext AuthContext { get; set; }
            public string ServerType { get; set; }
        }

        private void GetStatistics()
        {
            var serversList =
                ApiHelper.GetServersUri(ApiHelper.GetServers(this._mainServer), this._servicePort).Select(s => new ServerLoginContext()
                {
                    ServerType = "MainServer",
                    Uri = s
                })
                    .Concat(new[] { new ServerLoginContext()
                        {
                            ServerType = "RatingServer" ,
                            Uri = this.ratingServer
                        } })
                    .ToArray();

            this._serversAuthWithTypeContextList = this.DoAuthServers(serversList);
            this._statisticsUpdateTimer.Start();
        }

        private PasswordOAuthContext[] DoAuthServers(Uri[] serversList)
        {
            return Task.WhenAll(

                serversList.Select(
                        server =>
                            new PasswordAuthProvider(server))
                    .Select(async passwordProvider => await passwordProvider.Login(new PasswordAuthRequest<PasswordAuthData>
                    {
                        RequestData = new PasswordAuthData { Password = this._password, Usename = this._user }
                    }))).Result;
        }

        private ServerAuthContext[] DoAuthServers(ServerLoginContext[] contexts)
        {
            return Task.WhenAll(

                contexts.Select(
                        server =>
                            new Tuple<string, PasswordAuthProvider>(server.ServerType, new PasswordAuthProvider(server.Uri)))
                    .Select(async passwordProvider => new ServerAuthContext
                    {
                        AuthContext = await passwordProvider.Item2.Login(
                                                                                new PasswordAuthRequest<PasswordAuthData>
                                                                                {
                                                                                    RequestData =
                                                                                            new PasswordAuthData
                                                                                            {
                                                                                                Password
                                                                                                        =
                                                                                                        this
                                                                                                            ._password,
                                                                                                Usename
                                                                                                        =
                                                                                                        this
                                                                                                            ._user
                                                                                            }
                                                                                }),
                        ServerType = passwordProvider.Item1
                    })).Result;
        }

        class MultiServerOperationResult
        {
            public bool IsSuccess { get; set; }
        }

        class MultiServerOperationErrorResult : MultiServerOperationResult
        {
            public Dictionary<string, string> ServersErrors { get; set; }

            public MultiServerOperationErrorResult()
            {
                this.IsSuccess = false;
            }
        }

        private MultiServerOperationResult CheckServersAvailability(string[] serversList, int port)
        {
            var errors = new Dictionary<string, string>();

            serversList.Select(server => new AnonimousAuthContext()
            {
                BaseUri = new Uri(string.Format("http://{0}:{1}/", server, this._servicePort))
            }).Select(
                context =>
                {
                    var client = ApiHelper.GetServerInfoClient(context);
                    try
                    {
                        return client.Echo(context.BaseUri.Host);
                    }
                    catch (Exception e)
                    {
                        errors.Add(context.BaseUri.Host, e.Message);
                    }
                    return null;
                }
            );

            if (errors.Count > 0)
            {
                return new MultiServerOperationErrorResult()
                {
                    ServersErrors = errors
                };
            }

            return new MultiServerOperationResult() { IsSuccess = true };
        }

        public void GetStatisticsCallback(object sender, ElapsedEventArgs e)
        {
            var statistics = Task.WhenAll(
                this._serversAuthWithTypeContextList.Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
                    () =>
                    {
                        try
                        {
                            var result = new Tuple<Uri, ServerStatisticsInfoModel, string>(
                                _.AuthContext.BaseUri,
                                (
                                    _.ServerType == "MainServer" ? 
                                    ApiHelper.GetStatisticsClient<MainServerStatisticsInfoModel>(_.AuthContext) :
                                    ApiHelper.GetStatisticsClient<ServerStatisticsInfoModel>(_.AuthContext)
                                ).GetStatistics(), _.ServerType);
                            return result;
                        }
                        catch (Exception)
                        {
                            return new Tuple<Uri, ServerStatisticsInfoModel, string>(_.AuthContext.BaseUri, null, _.ServerType);
                        }
                    }))
            ).Result;

            lock (this._stdLock)
            {
                this.PrintStatistics(statistics);
            }
        }


        private void AddServer()
        {
            var serversList = ApiHelper.GetServers(this._mainServer).ToArray();
            Console.Clear();
            Console.WriteLine("Please enter server address:");
            var server = Console.ReadLine();

            serversList = serversList.Concat(new[] { server }).ToArray();

            var uriServersList = ApiHelper.GetServersUri(serversList, this._servicePort);

            var checkResult = this.CheckServersAvailability(serversList, this._servicePort);
            if (!checkResult.IsSuccess)
            {
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "Servers unavailable: {0}",
                    string.Join(", ", ((MultiServerOperationErrorResult)checkResult).ServersErrors.Keys));
                return;
            }
            try
            {
                var results = Task.WhenAll(

                this.DoAuthServers(uriServersList)
                    .Where(_ => _ != null)
                    .Select(
                        _ => Task.Factory.StartNew(
                            () =>
                            {
                                try
                                {

                                    ApiHelper.GetAdminClient(_).UpdateServersList(new[] { server });
                                }
                                catch (Exception)
                                {

                                }
                            }
                            )
                         ));
            }
            catch (AggregateException exception)
            {
                foreach (var innerException in exception.InnerExceptions)
                {
                    var clientException = innerException as IClientException<ErrorData>;
                    if (clientException != null)
                    {
                        ApiHelper.HandleClientException(clientException);
                    }
                }
            }
        }

        private void PrintStatistics(Tuple<Uri, ServerStatisticsInfoModel, string>[] statistics)
        {
            Console.Clear();
            foreach (var serverStatisticsInfoModel in statistics)
            {
                var mainServerInfoModel = serverStatisticsInfoModel.Item3 == "MainServer"
                              ? (MainServerStatisticsInfoModel)serverStatisticsInfoModel.Item2
                              : null;

                Console.WriteLine(serverStatisticsInfoModel.Item1);
                if (mainServerInfoModel != null)
                {
                    PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Games:");
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}", mainServerInfoModel.ActiveGamesCount);
                }

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Sessions:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} ",
                    serverStatisticsInfoModel.Item2.ActiveSessionsCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Uptime:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0:dd\\.hh\\:mm\\:ss}",
                    serverStatisticsInfoModel.Item2.ServerUptime);
                if (mainServerInfoModel != null)
                {
                    PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Users:");
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}",
                       mainServerInfoModel.RegistredUsersCount);

                    PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Calc/tck:");
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} ms",
                        mainServerInfoModel.PerformanceInfo != null
                            ? mainServerInfoModel.PerformanceInfo.CalculationsPerCycle.TotalMilliseconds
                            : 0);
                    PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Save/tck:");
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9} ",
                       mainServerInfoModel.PerformanceInfo != null
                            ? mainServerInfoModel.PerformanceInfo.PersistencePerCycle.TotalMilliseconds
                            : 0);
                }
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "OS:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, serverStatisticsInfoModel.Item2.SystemInfo.Os);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Runtime: ");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, serverStatisticsInfoModel.Item2.SystemInfo.Runtime);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "CPUs:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}",
                    serverStatisticsInfoModel.Item2.SystemInfo.CpusCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "RAM:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9} Mb",
                    serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "CPU:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-5:N1} %",
                    serverStatisticsInfoModel.Item2.ResourcesUsageInfo.CpuUsed);
                this.PrintMarker(serverStatisticsInfoModel.Item2.ResourcesUsageInfo.CpuUsed, 40);

                var memoryPercent = (serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal -
                                     serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryAvailable)
                                    / serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal * 100;
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "RAM Avail.: ");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} Mb",
                    serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryAvailable);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Process: ");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9:N1} Mb",
                    serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryUsedByProcess);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "RAM:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-5:N1} % ", memoryPercent);
                this.PrintMarker(memoryPercent, 40);
                PrettyConsole.WriteLineColor(ConsoleColor.DarkCyan, "\tWorkers");
                Array.ForEach(serverStatisticsInfoModel.Item2.ActiveWorkers,
                    w => PrettyConsole.WriteLineColor(ConsoleColor.DarkYellow, "\t\t{0}", w));
                Console.WriteLine();

            }
        }

        private void PrintMarker(float cpuUsage, int size)
        {
            var progressLines = (int)Math.Round(cpuUsage / 100 * size);
            Console.Write("\t[");
            var greenLines = 0;
            var yellowLines = 0;
            var redLines = 0;
            var empty = 0;
            if (cpuUsage < 80)
            {
                greenLines = progressLines;
            }
            else if (cpuUsage >= 80 && cpuUsage < 95)
            {
                greenLines = 80 * size / 100;
                yellowLines = progressLines - greenLines;
            }
            else if (cpuUsage > 95)
            {
                greenLines = 80 * size / 100;
                yellowLines = 15 * size / 100;
                redLines = progressLines - greenLines - yellowLines;
            }
            empty = size - progressLines;
            PrettyConsole.WriteColor(ConsoleColor.Green, new string('|', greenLines));
            PrettyConsole.WriteColor(ConsoleColor.Yellow, new string('|', yellowLines));
            PrettyConsole.WriteColor(ConsoleColor.Red, new string('|', redLines));
            PrettyConsole.WriteColor(ConsoleColor.DarkGray, new string(' ', empty));
            Console.WriteLine(']');
        }

        public void StartMenu()
        {
            Console.Clear();

            this._exitFlag = false;
            while (!this._exitFlag)
            {
                this._isMainMenu = true;
                this.ShowMenu();
                var key = Console.ReadKey();
                this._isMainMenu = false;
                Console.Clear();

                try
                {
                    this._menu[key.Key].MenuAction.Invoke();
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                    Console.WriteLine(exp.StackTrace);
                    //GetAssociatedDelegate(-1).Invoke();
                }

                if (!this._exitFlag)
                {
                    this.Pause();
                    Console.Clear();
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("-=Admin=-");
            foreach (var item in this._menu)
                Console.WriteLine(item.Key + " - " + item.Value.MenuText);
        }

        private void Pause()
        {
            Console.WriteLine("Paused. Press any key...");
            Console.ReadKey();
            this._statisticsUpdateTimer.Stop();
        }


        private void GetActiveSessions()
        {
            Console.Clear();

            var server = this.GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, this._servicePort)), this._user, this._password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);

            try
            {
                var activeSessionsInfos = client.GetActiveSessions().Result;
                foreach (var activeSessionsInfo in activeSessionsInfos)
                {
                    Console.WriteLine("Login: {0}", activeSessionsInfo.Login);

                    var i = 0;
                    foreach (var userSession in activeSessionsInfo.Sessions)
                    {
                        Console.WriteLine("\tSession №{0}. GUID: {1}", i, userSession.SessionIdentifier);
                        i++;
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseSessionByGuid()
        {
            Console.Clear();
            var server = this.GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, this._servicePort)), this._user, this._password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);


            Console.WriteLine("Enter GUID of session:");

            try
            {
                var guid = new Guid(Console.ReadLine());

                client.CloseSessionByGuid(guid);
                Console.WriteLine("Session {0} has been closed", guid);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseSessionByUserNameAndPositionOfGuid()
        {
            Console.Clear();

            var server = this.GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, this._servicePort)), this._user, this._password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);

            try
            {
                Console.WriteLine("Enter login:");
                var username = Console.ReadLine();

                Console.WriteLine("Enter position of (№) GUID:");
                var position = Convert.ToInt32(Console.ReadLine());

                var guid =
                    client
                        .GetActiveSessions().Result
                        .First(user => user.Login == username)
                        .Sessions[position].SessionIdentifier;
                client.CloseSessionByGuid(guid);

                Console.WriteLine("Session of user {0} {1} has been closed", guid, username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseAllUserSessions()
        {
            Console.Clear();

            var server = this.GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, this._servicePort)), this._user, this._password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);

            Console.WriteLine("Enter login to close sessions:");

            try
            {
                var username = Console.ReadLine();

                client.CloseAllUserSessions(username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseAllSessions()
        {
            Console.Clear();
            Console.WriteLine("Close sessions...");

            var server = this.GetServer();

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, this._servicePort)), this._user, this._password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);

            try
            {
                client.CloseAllSessions();
                Console.WriteLine("Session for server {0} are closed!", server);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseAllSessionsExludeThis()
        {
            var server = this.GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, this._servicePort)), this._user, this._password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);

            try
            {
                client.CloseAllSessionsExcludeThis(this.SessionGuid);
                Console.WriteLine("Круто!");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void WipeUser()
        {
            Console.Clear();
            var server = this.GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, this._servicePort)), this._user, this._password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);

            Console.WriteLine("Please enter username for wipe him:");

            try
            {
                var username = Console.ReadLine();

                Console.WriteLine("All user data will lost, continue??? (Y/N):");
                if (Console.ReadLine().ToUpper() != "Y")
                    return;

                client.WipeUser(username);

                Console.WriteLine("User {0} has been wiped", username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
            finally
            {
                ApiHelper.UserLogout(auth);
            }
        }

        private void Exit()
        {
            this._exitFlag = true;
        }

        private string GetServer()
        {
            string[] serversList = null;
            try
            {
                serversList = ApiHelper.GetServers(this._mainServer).ToArray();
            }
            catch (AggregateException e)
            {
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "Main server: {0}", this._mainServer);
                foreach (var innerException in e.InnerExceptions)
                {
                    var clientException = innerException as IClientException<ErrorData>;
                    if (clientException != null)
                    {
                        ApiHelper.HandleClientException(clientException);
                    }
                    else
                    {
                        throw;
                    }

                }
            }
            if (serversList == null) return string.Empty;
            PrettyConsole.WriteLineColor(ConsoleColor.Blue, "Servers list: {0}", string.Join(", ", serversList));
            Console.Write("Please enter server number: ");
            var serverNumberString = Console.ReadLine();
            return serversList[int.Parse(serverNumberString)];
        }
    }
}