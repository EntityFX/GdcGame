using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using RestSharp.Portable.Authenticators.OAuth2.Infrastructure;

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
        private readonly Lazy<PasswordOAuthContext[]> _serversAuthContextsList;
        private readonly string[] _serversList;
        private readonly int _servicePort;
        private readonly string _user;


        private IAdminController _adminManagerClient;
        //Dictionary<string, Delegate> _menu;
        private bool _exitFlag;

        private Dictionary<ConsoleKey, MenuItem> _menu;

        public AdminConsole(string user, string password, PasswordOAuthContext serverContext,
            IAdminController adminManagerClient, string[] serversList, int servicePort)
        {
            _user = user;
            _password = password;
            _serverContext = serverContext;
            _serversList = serversList;
            _servicePort = servicePort;

            _serversAuthContextsList = new Lazy<PasswordOAuthContext[]>(() =>
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

            });

            SetAdminClient(adminManagerClient);
            InitMenu();
        }

        private Guid SessionGuid { get; }

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
                {ConsoleKey.Escape, new MenuItem {MenuText = "Выход", MenuAction = Exit}}
            };
        }

        private void Echo()
        {
            var echoResults = Task.WhenAll(
                _serversAuthContextsList.Value.Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
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
            while (!_exitFlag)
            {
                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    _exitFlag = true;
                    break;
                }
                var statistics = Task.WhenAll(
               _serversAuthContextsList.Value.Where(_ => _ != null).Select(_ => Task.Factory.StartNew(
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

                Console.Clear();
                Console.WriteLine("+{0}+", new string('-', 73));
                Console.WriteLine("|{0,24}|{1,7}|{2,8}|{3,11}|{4,9}|{5,9}|", "Сервер", "Игр", "Сессий", "Uptime", "Расч/цикл", "Сохр/цикл");
                Console.WriteLine("+{0}+", new string('-', 73));
                foreach (var serverStatisticsInfoModel in statistics)
                {
                    Console.Write("|{0,-24}|", serverStatisticsInfoModel.Item1);
                    if (serverStatisticsInfoModel.Item2 != null)
                    {
                        Console.WriteLine("{0,7}|{1,8}|{2,11:hh\\:mm\\:ss}|{3,9}|{4,9}|"
                            , serverStatisticsInfoModel.Item2.ActiveGamesCount
                            , serverStatisticsInfoModel.Item2.ActiveSessionsCount
                            , serverStatisticsInfoModel.Item2.ServerUptime
                            , serverStatisticsInfoModel.Item2.PerformanceInfo.CalculationsPerCycle.TotalMilliseconds
                            , serverStatisticsInfoModel.Item2.PerformanceInfo.PersistencePerCycle.TotalMilliseconds
                        );
                    }
                    else
                    {
                        Console.WriteLine("{0,45}|", "Недоступен");
                    }
                    Console.WriteLine("+{0}+", new string('-', 73));
                }
                Thread.Sleep(5000);
            }


        }

        public void StartMenu()
        {
            Console.Clear();

            _exitFlag = false;
            while (!_exitFlag)
            {
                ShowMenu();
                var key = Console.ReadKey();
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