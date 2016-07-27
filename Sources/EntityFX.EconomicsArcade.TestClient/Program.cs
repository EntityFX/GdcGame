using System;
using System.Configuration;
using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Manager;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using IclServices.WcfTest.TestClient;
using PortableLog.NLog;

namespace EntityFX.EconomicsArcade.TestClient
{
    internal class Program
    {
        private static Guid _sessionGuid;

        private static string _userName;

        private static void Main(string[] args)
        {
            MainLoop(args);

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

            var simpleUserManagerClient = new SimpleUserManagerClient<NetTcpProxy<ISimpleUserManager>> (
                ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"]
                );
            if (!simpleUserManagerClient.Exists(userName))
            {
                simpleUserManagerClient.Create(userName);
            }

            var sessionManagerClient = new SessionManagerClient<NetTcpProxy<ISessionManager>>(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"], Guid.Empty
                );
            return new Tuple<Guid, string>(sessionManagerClient.OpenSession(userName), userName);
        }

        private static bool UserLogout(Guid session)
        {
            var sessionManagerClient = new SessionManagerClient<NetTcpProxy<ISessionManager>>(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"], session
                );
            return sessionManagerClient.CloseSession();
        }

        private static IGameManager GetGameClient(Guid sessionGuid)
        {
            return new GameManagerClient<NetTcpProxy<IGameManager>>(
                new Logger(new NLoggerAdapter(new NLogLogExFactory().GetLogger("logger")))
                , ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"]
                , sessionGuid
                );
        }

        private static IAdminManager GetAdminClient(Guid sessionGuid)
        {
            return new AdminManagerClient<NetTcpProxy<IAdminManager>>(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_AdminManager"], _sessionGuid
                );
        }

        private static void MainLoop(string[] args)
        {
            if (args.Length > 0)
            {
                _userName = args[0];
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