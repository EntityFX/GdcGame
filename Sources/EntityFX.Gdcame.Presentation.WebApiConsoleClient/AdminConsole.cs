using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Timers;
using System.Web.UI.WebControls;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Infrastructure.Api.Auth;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    internal class MenuItem
    {
        public string MenuText { get; set; }

        public Action MenuAction { get; set; }
    }


    internal class AdminConsole
    {
        private readonly string _password;
        private readonly PasswordOAuthContext _serverContext;
        private readonly string[] _serversList;
        private readonly int _servicePort;
        private readonly string _user;


        private IAdminController _adminManagerClient;
        //Dictionary<string, Delegate> _menu;
        private bool _exitFlag;
        private bool _isMainMenu;

        private Dictionary<ConsoleKey, MenuItem> _menu;

        private Timer _statisticsUpdateTimer;

        private PasswordOAuthContext[] _serversAuthContextList;
        private object _stdLock = new {};

        public AdminConsole(string user, string password, PasswordOAuthContext serverContext,
            IAdminController adminManagerClient, string[] serversList, int servicePort)
        {
            _user = user;
            _password = password;
            _serverContext = serverContext;
            _serversList = serversList;
            _servicePort = servicePort;


            _statisticsUpdateTimer = new Timer(5000);
            _statisticsUpdateTimer.Elapsed += GetStatisticsCallback;

            SetAdminClient(adminManagerClient);
            InitMenu();
        }


        private Guid SessionGuid { get;  set; }

        private void SetAdminClient(IAdminController adminManagerClient)
        {
            _adminManagerClient = adminManagerClient;
        }

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
                {ConsoleKey.Backspace, new MenuItem {MenuText = "Закрыть все игры", MenuAction = StopAllGames}},
                {ConsoleKey.Escape, new MenuItem {MenuText = "Выход", MenuAction = Exit}}
            };
        }

        private void StopAllGames()
        {
            Task.WhenAll(DoAuthServers().Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
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
            var echoResults = Task.WhenAll(
                DoAuthServers().Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
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
            _serversAuthContextList = DoAuthServers();
            _statisticsUpdateTimer.Start();
        }

        private PasswordOAuthContext[] DoAuthServers()
        {
            return Task.WhenAll(

                    _serversList.Select(
                            server =>
                                new PasswordAuthProvider(
                                    new Uri(string.Format("http://{0}:{1}/", server, _servicePort))))
                        .Select(async passwordProvider =>
                        {
                            try
                            {
                                return await passwordProvider.Login(new PasswordAuthRequest<PasswordAuthData>
                                {
                                    RequestData = new PasswordAuthData { Password = _password, Usename = _user }
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

        private void PrintStatistics(Tuple<Uri, ServerStatisticsInfoModel>[] statistics)
        {
            Console.Clear();
            foreach (var serverStatisticsInfoModel in statistics)
            {
                Console.WriteLine(serverStatisticsInfoModel.Item1);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Игр:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}", serverStatisticsInfoModel.Item2.ActiveGamesCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Сессий:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} ", serverStatisticsInfoModel.Item2.ActiveSessionsCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-10}", "Uptime:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0:dd\\.hh\\:mm\\:ss}", serverStatisticsInfoModel.Item2.ServerUptime);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Пользовтелей:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}", serverStatisticsInfoModel.Item2.RegistredUsersCount);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Вычисл / цикл:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} ms", serverStatisticsInfoModel.Item2.PerformanceInfo != null ? serverStatisticsInfoModel.Item2.PerformanceInfo.CalculationsPerCycle.TotalMilliseconds : 0);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Сохран / цикл:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9} ", serverStatisticsInfoModel.Item2.PerformanceInfo != null ? serverStatisticsInfoModel.Item2.PerformanceInfo.PersistencePerCycle.TotalMilliseconds : 0);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "OS:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, serverStatisticsInfoModel.Item2.SystemInfo.Os);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Runtime: ");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, serverStatisticsInfoModel.Item2.SystemInfo.Runtime);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "CPUs:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9}", serverStatisticsInfoModel.Item2.SystemInfo.CpusCount);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "RAM:");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9} Mb", serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal);

                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "CPU:");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-5:N1} %", serverStatisticsInfoModel.Item2.ResourcesUsageInfo.CpuUsed);
                PrintMarker(serverStatisticsInfoModel.Item2.ResourcesUsageInfo.CpuUsed, 50);

                var memoryPercent = (serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal -
                                     serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryAvailable)
                                    / serverStatisticsInfoModel.Item2.SystemInfo.MemoryTotal * 100;
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "RAM Доступно: ");
                PrettyConsole.WriteColor(ConsoleColor.Cyan, "{0,-9} Mb", serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryAvailable);
                PrettyConsole.WriteColor(ConsoleColor.DarkCyan, "\t{0,-15}", "Процесс: ");
                PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{0,-9:N1} Mb", serverStatisticsInfoModel.Item2.ResourcesUsageInfo.MemoryUsedByProcess);

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
            Console.WriteLine("-=Администрирование=-");
            foreach (var item in _menu)
                Console.WriteLine(item.Key + " - " + item.Value.MenuText);
        }

        private void Pause()
        {
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
            _statisticsUpdateTimer.Stop();
        }


        private void GetActiveSessions()
        {
            Console.Clear();

            try
            {
                var activeSessionsInfos = _adminManagerClient.GetActiveSessions().Result;
                foreach (var activeSessionsInfo in activeSessionsInfos)
                {
                    Console.WriteLine("Логин: {0}", activeSessionsInfo.Login);

                    var i = 0;
                    foreach (var userSession in activeSessionsInfo.Sessions)
                    {
                        Console.WriteLine("\tСессия №{0}. GUID: {1}", i, userSession.SessionIdentifier);
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
            Console.WriteLine("Введите GUID сессии для закрытия:");

            try
            {
                var guid = new Guid(Console.ReadLine());

                _adminManagerClient.CloseSessionByGuid(guid);
                Console.WriteLine("Сессия {0} закрыта", guid);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseSessionByUserNameAndPositionOfGuid()
        {
            Console.Clear();

            try
            {
                Console.WriteLine("Введите логин:");
                var username = Console.ReadLine();

                Console.WriteLine("Введите позицию (№) GUID:");
                var position = Convert.ToInt32(Console.ReadLine());

                var guid =
                    _adminManagerClient
                        .GetActiveSessions().Result
                        .First(user => user.Login == username)
                        .Sessions[position].SessionIdentifier;
                _adminManagerClient.CloseSessionByGuid(guid);

                Console.WriteLine("Сессия {0} пользователя {1} закрыта", guid, username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseAllUserSessions()
        {
            Console.Clear();
            Console.WriteLine("Введите логин для закрытия всех его сессий:");

            try
            {
                var username = Console.ReadLine();

                _adminManagerClient.CloseAllUserSessions(username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseAllSessions()
        {
            Console.Clear();
            Console.WriteLine("Закрываем сессии...");

            try
            {
                _adminManagerClient.CloseAllSessions();
                Console.WriteLine("Круто!");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseAllSessionsExludeThis()
        {
            try
            {
                _adminManagerClient.CloseAllSessionsExcludeThis(SessionGuid);
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
            Console.WriteLine("Please enter username for wipe him:");

            try
            {
                var username = Console.ReadLine();

                Console.WriteLine("Все данные пользователя будут обнулены. Продолжить??? (Y/N):");
                if (Console.ReadLine().ToUpper() != "Y")
                    return;

                _adminManagerClient.WipeUser(username);

                Console.WriteLine("Пользователь {0} обнулён", username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void Exit()
        {
            _exitFlag = true;
        }
    }
}