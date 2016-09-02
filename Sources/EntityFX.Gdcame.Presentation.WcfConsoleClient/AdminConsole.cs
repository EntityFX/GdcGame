using System;
using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Manager.Contract.AdminManager;

namespace EntityFX.Gdcame.Presentation.WcfConsoleClient
{
    internal class AdminConsole
    {
        private IAdminManager _adminManagerClient;
        //Dictionary<string, Delegate> _menu;
        private bool _exitFlag;

        private Dictionary<int, string> _menu;

        public AdminConsole(IAdminManager adminManagerClient, Guid currentGuid)
        {
            _adminManagerClient = adminManagerClient;
            SessionGuid = currentGuid;

            InitMenu();
        }

        public Guid SessionGuid { get; set; }

        public void SetAdminClient(IAdminManager adminManagerClient)
        {
            _adminManagerClient = adminManagerClient;
        }

        private void InitMenu()
        {
            _menu = new Dictionary<int, string>();

            //CallDelegate abc = new CallDelegate(CreateUser);
            _menu.Add(1, "Get active sessions and games");
            //new CallDelegate(CreateUser));
            _menu.Add(2, "Close session by GUID");
            _menu.Add(3, "Close session by username and position of GUID");
            _menu.Add(4, "Close all user sessions");
            _menu.Add(5, "Close all sessions");
            _menu.Add(6, "Close all sessions exlude this");
            _menu.Add(7, "Wipe user");
            _menu.Add(8, "Exit");
        }

        public void StartMenu()
        {
            Console.Clear();

            _exitFlag = false;
            while (!_exitFlag)
            {
                ShowMenu();
                var key = Console.ReadLine();
                Console.Clear();

                try
                {
                    GetAssociatedDelegate(
                        Convert.ToInt32(key)).Invoke();
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
            Console.WriteLine("Choose line and enter number:");
            //for (int i = 0; i < _menu.Count; i++)
            //    Console.WriteLine((i + 1) + ". " + _menu[i]);
            foreach (var item in _menu)
            {
                Console.WriteLine(item.Key + ". " + item.Value);
            }
        }

        private void Pause()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private CallDelegate GetAssociatedDelegate(int numberOfRow)
        {
            switch (numberOfRow)
            {
                case 1:
                    return GetActiveSessions;
                case 2:
                    return CloseSessionByGuid;
                case 3:
                    return CloseSessionByUserNameAndPositionOfGuid;
                case 4:
                    return CloseAllUserSessions;
                case 5:
                    return CloseAllSessions;
                case 6:
                    return CloseAllSessionsExludeThis;
                case 7:
                    return WipeUser;
                default:
                    return Exit;
            }
        }

        private void GetActiveSessions()
        {
            Console.Clear();

            try
            {
                var activeSessionsInfos = _adminManagerClient.GetActiveSessions();
                foreach (var activeSessionsInfo in activeSessionsInfos)
                {
                    Console.WriteLine("Username: {0}", activeSessionsInfo.UserName);

                    var i = 0;
                    foreach (var userSession in activeSessionsInfo.UserSessions)
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
                        .GetActiveSessions()
                        .First(user => user.UserName == username)
                        .UserSessions[position].SessionIdentifier;
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

        private delegate void CallDelegate();
    }
}