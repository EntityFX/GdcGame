using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Presentation.Contract.Controller;

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
                    new MenuItem {MenuText = "Get active sessions and games", MenuAction = GetActiveSessions}
                },
                {ConsoleKey.F2, new MenuItem {MenuText = "Close session by GUID", MenuAction = CloseSessionByGuid}},
                {
                    ConsoleKey.F3,
                    new MenuItem
                    {
                        MenuText = "Close session by username and position of GUID",
                        MenuAction = CloseSessionByUserNameAndPositionOfGuid
                    }
                },
                {ConsoleKey.F4, new MenuItem {MenuText = "Close all user sessions", MenuAction = CloseAllUserSessions}},
                {ConsoleKey.F5, new MenuItem {MenuText = "Close all sessions", MenuAction = CloseAllSessions}},
                {
                    ConsoleKey.F6,
                    new MenuItem {MenuText = "Close all sessions exlude this", MenuAction = CloseAllSessionsExludeThis}
                },
                {ConsoleKey.F7, new MenuItem {MenuText = "Wipe user", MenuAction = WipeUser}},
                {ConsoleKey.Escape, new MenuItem {MenuText = "Exit", MenuAction = Exit}}
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
            Console.WriteLine("-=Admin Functions=-");
            foreach (var item in _menu)
            {
                Console.WriteLine(item.Key + " - " + item.Value.MenuText);
            }
        }

        private void Pause()
        {
            Console.WriteLine("Press any key to continue...");
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
                    Console.WriteLine("User login: {0}", activeSessionsInfo.Login);

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
            Console.WriteLine("Please enter session guid for close:");

            try
            {
                var guid = new Guid(Console.ReadLine());

                _adminManagerClient.CloseSessionByGuid(guid);
                Console.WriteLine("Session {0} is closed", guid);
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
                Console.WriteLine("Please enter username:");
                var username = Console.ReadLine();

                Console.WriteLine("Please enter position(№) of GUID:");
                var position = Convert.ToInt32(Console.ReadLine());

                var guid =
                    _adminManagerClient
                        .GetActiveSessions().Result
                        .First(user => user.Login == username)
                        .Sessions[position].SessionIdentifier;
                _adminManagerClient.CloseSessionByGuid(guid);

                Console.WriteLine("Session {0} of user {1} is closed", guid, username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void CloseAllUserSessions()
        {
            Console.Clear();
            Console.WriteLine("Please enter username for close his sessions:");

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
            Console.WriteLine("Trying to close all sessions...");

            try
             {
                 _adminManagerClient.CloseAllSessions();
                 Console.WriteLine("Success!");
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
                 Console.WriteLine("Success!");
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

                 Console.WriteLine("All proress will LOSTED. Do you sure??? (Y/N):");
                 if (Console.ReadLine().ToUpper() != "Y")
                     return;

                 _adminManagerClient.WipeUser(username);

                 Console.WriteLine("Progress of user {0} is wiped", username);
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