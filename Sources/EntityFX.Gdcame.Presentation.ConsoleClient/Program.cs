using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using EntityFx.EconomicsArcade.Test.Shared;
using EntityFX.Gdcame.Manager.Contract;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Presentation.ConsoleClient
{
    internal class Program
    {
        private static Guid _sessionGuid;

        private static string _userName;
        private static UnityContainer _container;

        private static void Main(string[] args)
        {
            var listArgs = args.ToList();
            bool isCollapsed = false;
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    isCollapsed = arg.Contains("IsCollapsed");
                    listArgs.Remove(arg);
                }
            }
            _container = new UnityContainer();
            var containerBootstrapper = new ContainerBootstrapper(isCollapsed).Configure(_container);
            MainLoop(listArgs);
            //Console.ReadKey();
        }

        private static Tuple<Guid, string> UserLogin(string userName)
        {
            Console.Clear();
            if (string.IsNullOrEmpty(userName))
            {
                Console.Write("Please, enter user name: ");
                userName = Console.ReadLine();
            }

            var simpleUserManagerClient = _container.Resolve<ISimpleUserManager>();
            if (!simpleUserManagerClient.Exists(userName))
            {
                simpleUserManagerClient.Create(new UserData() {Login = userName});
            }

            var sessionManagerClient = _container.Resolve<ISessionManager>(new ParameterOverride("sessionGuid", Guid.Empty));
            return new Tuple<Guid, string>(sessionManagerClient.OpenSession(userName), userName);
        }

        private static bool UserLogout(Guid session)
        {
            var sessionManagerClient = _container.Resolve<ISessionManager>(new ParameterOverride("sessionGuid", session));
            return sessionManagerClient.CloseSession();
        }

        private static IGameManager GetGameClient(Guid sessionGuid)
        {
            return _container.Resolve<IGameManager>(new ParameterOverride("sesionGuid", sessionGuid));
        }

        private static IAdminManager GetAdminClient(Guid sessionGuid)
        {
            return _container.Resolve<IAdminManager>(new ParameterOverride("sessionGuid", sessionGuid));
        }

        private static void MainLoop(IEnumerable<string> args)
        {
            if (args.Any())
            {
                _userName = args.First();
            }

            var loginResult = UserLogin(_userName);
            _sessionGuid = loginResult.Item1;
            _userName = loginResult.Item2;
            var gameClient = GetGameClient(_sessionGuid);

            var adminManagerClient = GetAdminClient(_sessionGuid);

            var gr = new GameRunner(_userName, _sessionGuid, gameClient);
            var ac = new AdminConsole(adminManagerClient, _sessionGuid);
            var gameData = gr.GetGameData();
            gr.DisplayGameData(gameData);
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                try
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        gr.PerformManualStep();
                    }
                    else if ((int) keyInfo.Key >= 65 && (int) keyInfo.Key <= 90)
                    {
                        gr.BuyFundDriver(keyInfo);
                    }
                    else if (keyInfo.Key == ConsoleKey.Multiply)
                    {
                        gr.FightAgainstCorruption();
                    }
                    else if (keyInfo.Key == ConsoleKey.Add)
                    {
                        gr.PerformFiveYearPlan();
                    }
                    else if (keyInfo.Key == ConsoleKey.F5)
                    {
                        gr.DisplayGameData(gr.GetGameData());
                    }
                    else if (keyInfo.Key == ConsoleKey.F2)
                    {
                        ac.StartMenu();

                        gr.Invalidate();
                    }                   
                    else if (keyInfo.Key == ConsoleKey.F3)
                    {
                        UserLogout(_sessionGuid);
                        gr.DisplayGameData(gr.GetGameData());
                    }
                }
                catch (FaultException<InvalidSessionFault> faultException)
                {
                    var res = UserLogin(null);
                    _sessionGuid = res.Item1;
                    _userName = res.Item2;
                    gr.SessionGuid = _sessionGuid;
                    ac.SessionGuid = _sessionGuid;
                    gr.User = _userName;
                    gr.SetGameClient(GetGameClient(_sessionGuid));
                    ac.SetAdminClient(GetAdminClient(_sessionGuid));
                    gr.DisplayGameData(gr.GetGameData());
                }
            }
        }
    }
}