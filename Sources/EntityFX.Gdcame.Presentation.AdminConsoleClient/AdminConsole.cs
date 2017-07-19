using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using System.Timers;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    internal class AdminConsole
    {
        private readonly string _password;
        private readonly Uri _mainServer;



        private readonly int _servicePort;
        private readonly string _user;


        //Dictionary<string, Delegate> _menu;
        private bool _exitFlag;

        private Dictionary<ConsoleKey, MenuItem> _menu;

        private Timer _statisticsUpdateTimer;

        private PasswordOAuthContext[] _serversAuthContextList;
        private object _stdLock = new { };
        private bool _isMainMenu;

        public AdminConsole(string user, string password, Uri mainServer, int port)

        {
            _user = user;
            _password = password;
            _mainServer = mainServer;
            _servicePort = port;
            _statisticsUpdateTimer = new Timer(5000);
            _statisticsUpdateTimer.Elapsed += GetStatisticsCallback;

            InitMenu();
        }


        private Guid SessionGuid { get; set; }

        private void InitMenu()
        {
            _menu = new Dictionary<ConsoleKey, MenuItem>
            {
                {
                    ConsoleKey.F1,
                    new MenuItem {MenuText = "Все активные сессии", MenuAction = GetActiveSessions}
                },
                {ConsoleKey.F2, new MenuItem {MenuText = "Закрыть сессию по GUID", MenuAction = CloseSessionByGuid}},
                {
                    ConsoleKey.F3,
                    new MenuItem
                    {
                        MenuText = "Закрыть сессию по имени пользователя и номеру в списке GUID",
                        MenuAction = CloseSessionByUserNameAndPositionOfGuid
                    }
                },
                {
                    ConsoleKey.F4,
                    new MenuItem {MenuText = "Закрыть все сессии пользователя", MenuAction = CloseAllUserSessions}
                },
                {ConsoleKey.F5, new MenuItem {MenuText = "Закрыть все сессии", MenuAction = CloseAllSessions}},
                {
                    ConsoleKey.F6,
                    new MenuItem
                    {
                        MenuText = "Закрыть все сессии кроме текущей",
                        MenuAction = CloseAllSessionsExludeThis
                    }
                },
                {ConsoleKey.F7, new MenuItem {MenuText = "Обнулить пользователя", MenuAction = WipeUser}},
                {ConsoleKey.F8, new MenuItem {MenuText = "Статистика", MenuAction = GetStatistics}},
                {ConsoleKey.F9, new MenuItem {MenuText = "Echo", MenuAction = Echo}},
                {ConsoleKey.F10, new MenuItem {MenuText = "Обновить список серверов", MenuAction = GetNodesList}},
                {ConsoleKey.F11, new MenuItem {MenuText = "Добавить сервер", MenuAction = AddServer}},
                {ConsoleKey.Backspace, new MenuItem {MenuText = "Закрыть все игры", MenuAction = StopAllGames}},
                {ConsoleKey.Escape, new MenuItem {MenuText = "Выход", MenuAction = Exit}}
            };
        }

        private void GetNodesList()
        {

        }

        private void StopAllGames()
        {
            var serversList = ApiHelper.GetServers(_mainServer).ToArray();

            Task.WhenAll(DoAuthServers(serversList).Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
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
            var serversList = ApiHelper.GetServers(_mainServer).ToArray();

            var echoResults = Task.WhenAll(
                DoAuthServers(serversList).Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
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

        private void GetStatistics()
        {
            var serversList = ApiHelper.GetServers(_mainServer).ToArray();

            _serversAuthContextList = DoAuthServers(serversList);
            _statisticsUpdateTimer.Start();
        }

        private PasswordOAuthContext[] DoAuthServers(string[] serversList)
        {
            return Task.WhenAll(

                serversList.Select(
                        server =>
                            new PasswordAuthProvider(
                                new Uri(string.Format("http://{0}:{1}/", server, _servicePort))))
                    .Select(async passwordProvider => await passwordProvider.Login(new PasswordAuthRequest<PasswordAuthData>
                    {
                        RequestData = new PasswordAuthData { Password = _password, Usename = _user }
                    }))).Result;
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
                IsSuccess = false;
            }
        }

        private MultiServerOperationResult CheckServersAvailability(string[] serversList, int port)
        {
            var errors = new Dictionary<string, string>();

            serversList.Select(server => new AnonimousAuthContext()
            {
                BaseUri = new Uri(string.Format("http://{0}:{1}/", server, _servicePort))
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
                _serversAuthContextList.Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
                    () =>
                    {
                        try
                        {
                            var result = new Tuple<Uri, ServerStatisticsInfoModel>(_.BaseUri,
                                ApiHelper.GetAdminClient(_).GetStatistics());
                            return result;
                        }
                        catch (Exception)
                        {
                            return new Tuple<Uri, ServerStatisticsInfoModel>(_.BaseUri, null);
                        }
                    }))
            ).Result;

            lock (_stdLock)
            {
                PrintStatistics(statistics);
            }
        }


        private void AddServer()
        {
            var serversList = ApiHelper.GetServers(_mainServer).ToArray();
            Console.Clear();
            Console.WriteLine("Please enter server address:");
            var server = Console.ReadLine();

            serversList = serversList.Concat(new[] { server }).ToArray();

            var checkResult = CheckServersAvailability(serversList, _servicePort);
            if (!checkResult.IsSuccess)
            {
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "Servers unavailable: {0}",
                    string.Join(", ", ((MultiServerOperationErrorResult)checkResult).ServersErrors.Keys));
                return;
            }
            try
            {
                var results = Task.WhenAll(

                DoAuthServers(serversList)
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

        private void PrintStatistics(Tuple<Uri, ServerStatisticsInfoModel>[] statistics)
        {
            Console.Clear();
            foreach (var serverStatisticsInfoModel in statistics)
            {
                Console.WriteLine(serverStatisticsInfoModel.Item1);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Games:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}", serverStatisticsInfoModel.Item2.ActiveGamesCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Sessions:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} ",
                    serverStatisticsInfoModel.Item2.ActiveSessionsCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Uptime:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0:dd\\.hh\\:mm\\:ss}",
                    serverStatisticsInfoModel.Item2.ServerUptime);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Users:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}",
                    serverStatisticsInfoModel.Item2.RegistredUsersCount);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Calc / tick:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} ms",
                    serverStatisticsInfoModel.Item2.PerformanceInfo != null
                        ? serverStatisticsInfoModel.Item2.PerformanceInfo.CalculationsPerCycle.TotalMilliseconds
                        : 0);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Save / tick:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9} ",
                    serverStatisticsInfoModel.Item2.PerformanceInfo != null
                        ? serverStatisticsInfoModel.Item2.PerformanceInfo.PersistencePerCycle.TotalMilliseconds
                        : 0);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "OS:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, serverStatisticsInfoModel.Item2.SystemInfo.Os);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Runtime: ");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, serverStatisticsInfoModel.Item2.SystemInfo.Runtime);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "CPUs:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}",
                    serverStatisticsInfoModel.Item2.SystemInfo.CpusCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "RAM:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9} Mb",
                    serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "CPU:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-5:N1} %",
                    serverStatisticsInfoModel.Item2.ResourcesUsageInfo.CpuUsed);
                PrintMarker(serverStatisticsInfoModel.Item2.ResourcesUsageInfo.CpuUsed, 50);

                var memoryPercent = (serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal -
                                     serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryAvailable)
                                    / serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal * 100;
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "RAM Available: ");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} Mb",
                    serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryAvailable);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Process: ");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9:N1} Mb",
                    serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryUsedByProcess);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "RAM:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-5:N1} % ", memoryPercent);
                PrintMarker(memoryPercent, 50);
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

            _exitFlag = false;
            while (!_exitFlag)
            {
                _isMainMenu = true;
                ShowMenu();
                var key = Console.ReadKey();
                _isMainMenu = false;
                Console.Clear();

                try
                {
                    _menu[key.Key].MenuAction.Invoke();
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                    Console.WriteLine(exp.StackTrace);
                    //GetAssociatedDelegate(-1).Invoke();
                }

                if (!_exitFlag)
                {
                    Pause();
                    Console.Clear();
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("-=Admin=-");
            foreach (var item in _menu)
                Console.WriteLine(item.Key + " - " + item.Value.MenuText);
        }

        private void Pause()
        {
            Console.WriteLine("Paused. Press any key...");
            Console.ReadKey();
            _statisticsUpdateTimer.Stop();
        }


        private void GetActiveSessions()
        {
            Console.Clear();

            var server = GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, _servicePort)), _user, _password)
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
            var server = GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, _servicePort)), _user, _password)
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

            var server = GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, _servicePort)), _user, _password)
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

            var server = GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, _servicePort)), _user, _password)
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

            var server = GetServer();

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, _servicePort)), _user, _password)
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
            var server = GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, _servicePort)), _user, _password)
                    .Result;

            var client = ApiHelper.GetAdminClient(auth);

            try
            {
                client.CloseAllSessionsExcludeThis(SessionGuid);
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
            var server = GetServer();
            if (string.IsNullOrEmpty(server)) return;

            var auth =
                ApiHelper.LoginServer(new Uri(string.Format("http://{0}:{1}/", server, _servicePort)), _user, _password)
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
            _exitFlag = true;
        }

        private string GetServer()
        {
            string[] serversList = null;
            try
            {
                serversList = ApiHelper.GetServers(_mainServer).ToArray();
            }
            catch (AggregateException e)
            {
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "Main server: {0}", _mainServer);
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