using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EntityFx.GdCame.Presentation.Shared;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Contract.Common.UserRating;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Presentation.ConsoleClient;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;
using EntityFX.Gdcame.Utils.WebApiClient;
using NLog;
using RestSharp.Authenticators;
using Unity;

namespace EntityFX.Gdcame.Presentation.ClientShared
{
    public class GameConsole
    {
        private static string _userName;
        private static string _userPassword;
        private IIocContainer _container;
        private static bool _exitFlag;

        private static readonly Infrastructure.Common.ILogger logger = new NLoggerAdapter(LogManager.GetLogger("logger"));

        private static readonly ApiHelper<IAuthenticator> apiHelper = new ApiHelper<IAuthenticator>(logger,
            new RestsharpOAuth2ProviderFactory(logger),
            new RestsharpApiClientFactory());

        private static readonly Dictionary<ConsoleKey, MenuItem> _mainMenu = new Dictionary<ConsoleKey, MenuItem>
        {
            {
                ConsoleKey.F1,
                new MenuItem {MenuText = "Sign In & Play", MenuAction = TryLogin}
            },
            {
                ConsoleKey.F2,
                new MenuItem {MenuText = "Sign up", MenuAction = RegisterAccount}
            },
            {
                ConsoleKey.Escape,
                new MenuItem {MenuText = "Exit", MenuAction = ExitMainMenu}
            }
        };

        private static Settings _settings;

        public GameConsole(Settings settings)
        {
            _container = new UnityIocContainer(new UnityContainer());
            _settings = settings;
        }

        public static ErrorCodes? ErrorCode { get; private set; }

        private static void ExitMainMenu()
        {
            _exitFlag = true;
        }

        private static void RatingMenu(IRatingController ratingClient)
        {
            var ratingData = ratingClient.GetRaiting(500).Result;
            ConsoleKeyInfo keyInfo;

            WriteRatingHeader();

            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                WriteRatingHeader();
                try
                {
                    if (keyInfo.Key == ConsoleKey.A)
                        WriteRatings(ratingData.ManualStepsCount);
                    if (keyInfo.Key == ConsoleKey.S)
                        WriteRatings(ratingData.RootCounter);
                    if (keyInfo.Key == ConsoleKey.D)
                        WriteRatings(ratingData.TotalEarned);
                }
                catch
                {
                }
            }
        }

        private static void WriteRatings(TopStatisticsAggregate statisticsAggregate)
        {
            WriteRatingHeader(true);
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
                try
                {
                    if (keyInfo.Key == ConsoleKey.A)
                    {
                        WriteRatingHeader(true, "Day");
                        foreach (var data in statisticsAggregate.Day)
                            Console.WriteLine("{0,20} {1,20}", data.Login, data.Value);
                    }
                    if (keyInfo.Key == ConsoleKey.S)
                    {
                        WriteRatingHeader(true, "Week");
                        foreach (var data in statisticsAggregate.Week)
                            Console.WriteLine("{0,20} {1,20}", data.Login, data.Value);
                    }
                    if (keyInfo.Key == ConsoleKey.D)
                    {
                        WriteRatingHeader(true, "Total");
                        foreach (var data in statisticsAggregate.Total)
                            Console.WriteLine("{0,20} {1,20}", data.Login, data.Value);
                    }
                }
                catch
                {
                }
            WriteRatingHeader();

            //foreach (var data in statisticsAggregate.Total)
            //{
            //    var dataDayResult = Math.Round(statisticsAggregate.Day.Where((el) => el.UserId == data.UserId).First().Value,2);
            //    var dataWeekResult = Math.Round(statisticsAggregate.Week.Where((el) => el.UserId == data.UserId).First().Value,2);
            //    var dataTotalResult = Math.Round(data.Value, 2);
            //    Console.WriteLine(string.Format("{0,20} {1,20} {2,20} {3,20}", data.Login, dataDayResult, dataWeekResult, dataTotalResult));
            //}
        }

        private static void WriteRatingHeader(bool usePeriodHeaderd = false, string period = "")
        {
            Console.Clear();
            Console.WriteLine("-=Rating=-\n");

            if (!usePeriodHeaderd)
            {
                Console.WriteLine("Select rating type");
                Console.WriteLine("A - ManualSteps\tS - RootCounter\tD - TotalEarned");
            }
            else
            {
                Console.WriteLine("Select period");
                Console.WriteLine("A - Day\tS - Week\tD - Total");
            }
            if (period != "")
            {
                Console.WriteLine("{0}:\t {1}", "Selected period", period);
                Console.WriteLine("{0,20} {1,20}", "Login", "Value");
            }
        }

        private static void RegisterAccount()
        {
            Console.Clear();
            Console.WriteLine("-=Sign Up=-");
            Console.Write("Enter login: ");
            var userName = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            Console.Write("Reenter password: ");
            var confirmPassword = Console.ReadLine();

            var serverInfoUrl = apiHelper.GetApiServerUri(
                apiHelper.GetServers(new Uri($"{_settings.GameServer}:{_settings.GameServicePort}")), userName,
                    _settings.GameServicePort);

            var apiFactory = new RestsharpApiClientFactory();
            var authApi =
                new AuthApiClient(apiFactory.Build(new AnonymousAuthContext<IAuthenticator> {BaseUri = serverInfoUrl}));

            try
            {
                var result = authApi.Register(new Common.Application.Model.RegisterAccountModel
                {
                    Login = userName,
                    Password = password,
                    ConfirmPassword = confirmPassword
                }).Result;
            }
            catch (AggregateException loginException)
            {
                ErrorCode = apiHelper.HandleClientException(
                    loginException.InnerException as IClientException<ErrorData>);
            }
        }

        private static void EnterMainMenu()
        {
            _exitFlag = false;
            while (!_exitFlag)
            {
                Console.WriteLine("-=Main menu=-");
                foreach (var item in _mainMenu)
                    Console.WriteLine(item.Key + " - " + item.Value.MenuText);
                var key = Console.ReadKey();
                Console.Clear();

                try
                {
                    if (_mainMenu.ContainsKey(key.Key))
                        _mainMenu[key.Key].MenuAction.Invoke();
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, exp.Message);
                }
            }
        }

        public void MainLoop(IEnumerable<string> args)
        {
            var argsArray = args as string[] ?? args.ToArray();
            if (argsArray.Any())
            {
                _userName = argsArray.First();
                if (argsArray.Count() >= 2)
                    _userPassword = argsArray.ToArray()[1];
            }
            EnterMainMenu();
        }

        private static void TryLogin()
        {
            Tuple<IAuthContext<IAuthenticator>, string> loginResultTuple;
            if (string.IsNullOrEmpty(_userName))
            {
                Console.Write("Enter login: ");
                _userName = Console.ReadLine();

                Console.Write("Enter password: ");
                _userPassword = Console.ReadLine();
            }
            try
            {
                Console.Clear();
                Console.WriteLine("-=Sign In=-");
                loginResultTuple = apiHelper.UserLogin(
                    apiHelper.GetServers(new Uri($"{_settings.GameServer}:{_settings.GameServicePort}")), _settings.GameServicePort,
                    new PasswordOAuth2RequestData
                    {
                        Usename = _userName,
                        Password = _userPassword
                    }).Result;
            }
            catch (AggregateException loginException)
            {
                apiHelper.HandleClientException(loginException.InnerException as IClientException<ErrorData>);
                loginResultTuple = null;
                _userName = null;
            }

            if (loginResultTuple != null)
            {
                EnterGame(loginResultTuple);
                Console.Clear();
            }
        }


        private static void EnterGame(Tuple<IAuthContext<IAuthenticator>, string> loginContext)
        {
            Console.Clear();

            var gameClient = apiHelper.GetGameClient(loginContext.Item1);
            var gr = new GameRunner(apiHelper, loginContext.Item2, loginContext.Item1, gameClient);
            var adminManagerClient = apiHelper.GetAdminClient(loginContext.Item1);
            var apiFactory = new RestsharpApiClientFactory();

            var rc = apiHelper.GetRatingClient(new AnonymousAuthContext<IAuthenticator>
            {
                BaseUri = new Uri(string.Format("{0}:{1}", _settings.RatingServer, _settings.RatingServerPort))
            });
            gr.Invalidate();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
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
                        gr.Invalidate();
                    }
                    else if (keyInfo.Key == ConsoleKey.F2)
                    {
                        ErrorCode = apiHelper.UserLogout(gr.ServerContext);
                        _userName = null;
                        break;
                    }
                    else if (keyInfo.Key == ConsoleKey.F3)
                    {
                        RatingMenu(rc);
                    }
                    if (gr.ErrorCode != null)
                        if (gr.ErrorCode == ErrorCodes.InvalidSessionError || gr.ErrorCode == ErrorCodes.OtherError)
                        {
                            gr.Invalidate();
                            return;
                        }
                }

                catch (Exception ex)
                {
                    var exception = ex.Message;
                }
        }
    }
}