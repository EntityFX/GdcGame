using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.TestClient;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using Microsoft.Practices.Unity;


using PortableLog.NLog;

namespace IclServices.WcfTest.TestClient
{
    class UIConsole
    {
        private readonly IAdminManager _adminManagerClient;

        Dictionary<int, string> _menu;
        //Dictionary<string, Delegate> _menu;
        bool _exitFlag;



        delegate void CallDelegate();

        public UIConsole(IAdminManager adminManagerClient)
        {
            _adminManagerClient = adminManagerClient;

            InitMenu();
        }

        private void InitMenu()
        {
            _menu = new Dictionary<int, string>();

            //CallDelegate abc = new CallDelegate(CreateUser);
            _menu.Add(1, "Get active sessions and games");
            //new CallDelegate(CreateUser));
            _menu.Add(2, "Close session by GUID");
            _menu.Add(3, "Close session by username and position of GUID");
            _menu.Add(4, "CloseAllUserSessions");
            _menu.Add(5, "Wipe user");
            _menu.Add(6, "Exit");
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
                    return new CallDelegate(GetActiveSessions);
                case 2:
                    return new CallDelegate(CloseSessionByGuid);
                case 3:
                    return new CallDelegate(CloseSessionByUserNameAndPositionOfGuid);
                case 4:
                    return new CallDelegate(CloseAllUserSessions);
                case 5:
                    return new CallDelegate(WipeUser);
                default:
                    return new CallDelegate(Exit);
            }
        }

        void GetActiveSessions()
        {
            Console.Clear();

            try
            {
                UserSessionsInfo[] activeSessionsInfos = _adminManagerClient.GetActiveSessions();
                foreach (var activeSessionsInfo in activeSessionsInfos)
                {
                    Console.WriteLine("Username: {0}", activeSessionsInfo.UserName);

                    int i = 0;
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
        void CloseSessionByGuid()
        {
            Console.Clear();
            Console.WriteLine("Please enter session guid for close:");

            try
            {
                Guid guid = new Guid(Console.ReadLine());

                _adminManagerClient.CloseSessionByGuid(guid);
                Console.WriteLine("Session {0} is closed", guid);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        void CloseSessionByUserNameAndPositionOfGuid()
        {
            Console.Clear();

            try
            {
                Console.WriteLine("Please enter username:");
                string username = Console.ReadLine();

                Console.WriteLine("Please enter position(№) of GUID:");
                int position = Convert.ToInt32(Console.ReadLine());

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
        void CloseAllUserSessions()
        {
            Console.Clear();
            Console.WriteLine("Please enter username for close his sessions:");

            try
            {
                string username = Console.ReadLine();

                _adminManagerClient.CloseAllUserSessions(username);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        void WipeUser()
        {
            Console.Clear();
            Console.WriteLine("Please enter username for wipe him:");

            try
            {
                string username = Console.ReadLine();

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
