using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.TestClient;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using Microsoft.Practices.Unity;


using PortableLog.NLog;

namespace IclServices.WcfTest.TestClient
{
    class UIConsole
    {
        private string[] _args;
        Dictionary<int, string> _menu;
        //Dictionary<string, Delegate> _menu;
        bool _exitFlag;



        delegate void CallDelegate();

        public UIConsole(string[] args)
        {
            _args = args;

            InitMenu();
        }

        private void InitMenu()
        {
            _menu = new Dictionary<int, string>();

            //CallDelegate abc = new CallDelegate(CreateUser);
            _menu.Add(1, "Get active sessions and games");
            //new CallDelegate(CreateUser));
            _menu.Add(2, "Get ???");
            _menu.Add(3, "Reset session from GUID");
            _menu.Add(4, "Reset session from username");
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
                    GetAssociatedDelegate(-1).Invoke();
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

        private CallDelegate GetAssociatedDelegate(int numberOfRow)
        {
            switch (numberOfRow)
            {
                case 1:
                    return new CallDelegate(GetActiveSessionsAndGames);
                case 2:
                    return new CallDelegate(GetNothing);
                case 3:
                    return new CallDelegate(ResetSessionFromGUID);
                case 4:
                    return new CallDelegate(ResetSessionFromUsername);
                case 5:
                    return new CallDelegate(WipeUser);
                default:
                    return new CallDelegate(Exit);
            }
        }

        void GetActiveSessionsAndGames()
        {
            
        }
        void GetNothing()
        {
            
        }
        void ResetSessionFromGUID()
        {
            
        }
        void ResetSessionFromUsername()
        {
            
        }
        void WipeUser()
        {
            
        }

        private void Exit()
        {
            _exitFlag = true;
        }
    }
}
