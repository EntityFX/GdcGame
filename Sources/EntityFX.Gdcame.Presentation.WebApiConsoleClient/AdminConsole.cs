using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Application.Contract.Controller;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    internal class MenuItem
    {
        public string MenuText { get; set; }

        public Action MenuAction { get; set; }
    }


    internal class AdminConsole
    {



        private IAdminController _adminManagerClient;
        //Dictionary<string, Delegate> _menu;
        private bool _exitFlag;

        private Dictionary<ConsoleKey, MenuItem> _menu;

        public AdminConsole(IAdminController adminManagerClient)
        {
            SetAdminClient(adminManagerClient);
            InitMenu();
        }

        public Guid SessionGuid { get; set; }

        public void SetAdminClient(IAdminController adminManagerClient)
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
                {ConsoleKey.F4, new MenuItem {MenuText = "Закрыть все сессии пользователя", MenuAction = CloseAllUserSessions}},
                {ConsoleKey.F5, new MenuItem {MenuText = "Закрыть все сессии", MenuAction = CloseAllSessions}},
                {
                    ConsoleKey.F6,
                    new MenuItem {MenuText = "Закрыть все сессии кроме текущей", MenuAction = CloseAllSessionsExludeThis}
                },
                {ConsoleKey.F7, new MenuItem {MenuText = "Обнулить пользователя", MenuAction = WipeUser}},
                {ConsoleKey.Escape, new MenuItem {MenuText = "Выход", MenuAction = Exit}}
            };
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

        public void ShowMenu()
        {
            Console.WriteLine("-=Администрирование=-");
            foreach (var item in _menu)
            {
                Console.WriteLine(item.Key + " - " + item.Value.MenuText);
            }
        }

        private void Pause()
        {
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadLine();
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