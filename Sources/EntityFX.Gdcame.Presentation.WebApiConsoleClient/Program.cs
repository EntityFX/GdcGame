using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using Microsoft.Practices.Unity;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;
using EntityFX.Gdcame.Contract.Common.UserRating;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    class Program
    {
        private static string _userName;
        private static string _userPassword;
        private static UnityContainer _container;
        public static ErrorCodes? ErrorCode { get; private set; }
        private static bool _exitFlag;

        private static Dictionary<ConsoleKey, MenuItem> _mainMenu = new Dictionary<ConsoleKey, MenuItem>
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

        private static int _serverPort;
        private static string _mainServer;
        private static int ratingServerPort;
        private static string ratingServer;

        private static void ExitMainMenu()
        {
            _exitFlag = true;
        }

        private static void Main(string[] args)
        {
            Console.SetWindowSize(100, 50);
            var listArgs = args.ToList();
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    listArgs.Remove(arg);
                }
            }
            _container = new UnityContainer();
            _serverPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePort"]);
            _mainServer = ConfigurationManager.AppSettings["Server"];
            ratingServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["RatingServerPort"]);
            ratingServer = ConfigurationManager.AppSettings["RatingServer"];
            MainLoop(listArgs);

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
                    {
                        WriteRatings(ratingData.ManualStepsCount);
                    }
                    if (keyInfo.Key == ConsoleKey.S)
                    {
                        WriteRatings(ratingData.RootCounter);
                    }
                    if (keyInfo.Key == ConsoleKey.D)
                    {
                        WriteRatings(ratingData.TotalEarned);
                    }
                }
                catch { }
            }
        }

        private static void WriteRatings(TopStatisticsAggregate statisticsAggregate)
        {
            WriteRatingHeader(true);
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                try
                {
                    if (keyInfo.Key == ConsoleKey.A)
                    {
                        WriteRatingHeader(true, "Day");
                        foreach (var data in statisticsAggregate.Day)
                        {
                            Console.WriteLine(string.Format("{0,20} {1,20}", data.Login, data.Value));
                        }
                    }
                    if (keyInfo.Key == ConsoleKey.S)
                    {
                        WriteRatingHeader(true, "Week");
                        foreach (var data in statisticsAggregate.Week)
                        {
                            Console.WriteLine(string.Format("{0,20} {1,20}", data.Login, data.Value));
                        }
                    }
                    if (keyInfo.Key == ConsoleKey.D)
                    {
                        WriteRatingHeader(true, "Total");
                        foreach (var data in statisticsAggregate.Total)
                        {
                            Console.WriteLine(string.Format("{0,20} {1,20}", data.Login, data.Value));
                        }
                    }
                }
                catch { }
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
                Console.WriteLine(string.Format("{0}:\t {1}", "Selected period", period));
                Console.WriteLine(string.Format("{0,20} {1,20}", "Login", "Value"));
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

            var serverInfoUrl = ApiHelper.GetApiServerUri(ApiHelper.GetServers(new Uri(string.Format("{0}:{1}", _mainServer, _serverPort))), userName, _serverPort);

            var authApi = new AuthApiClient(new PasswordOAuthContext() { BaseUri = serverInfoUrl });

            try
            {
                var result = authApi.Register(new RegisterAccountModel()
                {
                    Login = userName,
                    Password = password,
                    ConfirmPassword = confirmPassword
                }).Result;
            }
            catch (AggregateException loginException)
            {
                ErrorCode = ApiHelper.HandleClientException(loginException.InnerException as IClientException<ErrorData>) as ErrorCodes?;
            }


        }

        private static void TryLogin()
        {
            Tuple<PasswordOAuthContext, string> loginResultTuple;
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
                loginResultTuple = ApiHelper.UserLogin(ApiHelper.GetServers(new Uri(string.Format("{0}:{1}", _mainServer, _serverPort))), _serverPort, _userName, _userPassword).Result;
            }
            catch (AggregateException loginException)
            {
                ApiHelper.HandleClientException(loginException.InnerException as IClientException<ErrorData>);
                loginResultTuple = null;
                _userName = null;
            }

            if (loginResultTuple != null)
            {
                EnterGame(loginResultTuple);
                Console.Clear();
            }
        }

        private static void EnterMainMenu()
        {
            _exitFlag = false;
            while (!_exitFlag)
            {
                Console.WriteLine("-=Main menu=-");
                foreach (var item in _mainMenu)
                {
                    Console.WriteLine(item.Key + " - " + item.Value.MenuText);
                }
                var key = Console.ReadKey();
                Console.Clear();

                try
                {
                    if (_mainMenu.ContainsKey(key.Key))
                    {
                        _mainMenu[key.Key].MenuAction.Invoke();
                    }
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, exp.Message);
                }
            }
        }

        private static void EnterGame(Tuple<PasswordOAuthContext, string> loginContext)
        {
            Console.Clear();

            var gameClient = ApiHelper.GetGameClient(loginContext.Item1);
            var gr = new GameRunner(loginContext.Item2, loginContext.Item1, gameClient);
            var adminManagerClient = ApiHelper.GetAdminClient(loginContext.Item1);

            var rc = ApiHelper.GetRatingClient(new PasswordOAuthContext() { BaseUri = new Uri(string.Format("{0}:{1}", ratingServer, ratingServerPort)) });
            gr.Invalidate();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                try
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        gr.PerformManualStep();
                    }
                    else if ((int)keyInfo.Key >= 65 && (int)keyInfo.Key <= 90)
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
                        ErrorCode = ApiHelper.UserLogout(gr.ServerContext);
                        _userName = null;
                        break;
                    }
                    else if (keyInfo.Key == ConsoleKey.F3)
                    {
                        RatingMenu(rc);
                    }
                    if (gr.ErrorCode != null)
                    {
                        if (gr.ErrorCode == ErrorCodes.InvalidSessionError || gr.ErrorCode == ErrorCodes.OtherError)
                        {
                            gr.Invalidate();
                            return;
                        }

                    }
                }

                catch (Exception ex)
                {
                    var exception = ex.Message;
                }
            }
        }

        private static void MainLoop(IEnumerable<string> args)
        {
            var argsArray = args as string[] ?? args.ToArray();
            if (argsArray.Any())
            {
                _userName = argsArray.First();
                if (argsArray.Count() >= 2)
                {
                    _userPassword = argsArray.ToArray()[1];
                }
            }
            EnterMainMenu();
        }

        internal abstract class GameRunnerBase
        {
            protected static readonly object _stdLock = new { };

            private bool IsFundsDriverAvailableForBuy(CounterModelBase rootCounter, ItemModel item)
            {
                return item.UnlockBalance <= rootCounter.Value;
            }

            private bool IsCounterAvailableForActivate(DelayedCounterModel counter, decimal currentFunds)
            {
                return counter.UnlockValue <= currentFunds;
            }

            private bool IsCounterWithInflation(GenericCounterModel counter)
            {
                return counter.Inflation > 0;
            }

            private bool IsCounterIsMining(DelayedCounterModel counter)
            {
                return counter.SecondsRemaining > 0;
            }

            private string GetIncrementorValueById(ItemModel item, int incrmentorId)
            {
                var incrementor = item.Incrementors.ContainsKey(incrmentorId)
                    ? item.Incrementors[incrmentorId]
                    : null;
                if (incrementor != null)
                {
                    return incrementor;
                }
                return "0";
            }

            public abstract GameDataModel GetGameData();

            public virtual void DisplayGameData(GameDataModel gameData)
            {
                lock (_stdLock)
                {
                    Console.WriteLine("Balance: {0:C}; Total earned: {1:C}", gameData.Cash.OnHand,
                        gameData.Cash.TotalEarned);
                    /*Console.WriteLine("Manual Steps: {0}, Automatic Steps: {1}",
                        gameData.ManualStepsCount, gameData.AutomatedStepsCount);*/
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "{1,15}: {0,12}", gameData.Cash.Counters[0].Value,
                        gameData.Cash.Counters[0].Name);
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ",
                        ((GenericCounterModel)gameData.Cash.Counters[1]).SubValue, gameData.Cash.Counters[1].Name);
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ({2}%)"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Bonus, "Bonus"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).BonusPercentage);
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "{1,15}: {0,12}%"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Inflation, "Buthurt");
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Value, "Total");
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}",
                        ((GenericCounterModel)gameData.Cash.Counters[2]).SubValue, gameData.Cash.Counters[2].Name);
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Bonus, "Bonus"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).BonusPercentage);
                    PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Inflation, "Fatigue");
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Value, "Total");
                    PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: +{0,12:C} {2}/{3}",
                        gameData.Cash.Counters[3].Value, gameData.Cash.Counters[3].Name
                        , TimeSpan.FromSeconds(((DelayedCounterModel)gameData.Cash.Counters[3]).SecondsRemaining)
                        , 0);

                    Console.WriteLine();
                    var charIndex = 65;
                    PrettyConsole.WriteLineColor(
                        IsCounterWithInflation((GenericCounterModel)gameData.Cash.Counters[2])
                            ? ConsoleColor.Yellow
                            : ConsoleColor.DarkYellow, "{0,2}:   Do rest", "*");
                    if (IsCounterAvailableForActivate((DelayedCounterModel)gameData.Cash.Counters[3],
                        gameData.Cash.Counters[0].Value))
                    {
                        PrettyConsole.WriteLineColor(
                            IsCounterIsMining((DelayedCounterModel)gameData.Cash.Counters[3])
                                ? ConsoleColor.DarkMagenta
                                : ConsoleColor.Magenta, "{0,2}:         Do quarter goal", "+");
                    }
                    else
                    {
                        PrettyConsole.WriteLineColor(ConsoleColor.DarkMagenta,
                            "{0,2}:         To do quarter goal need {1} {2}", "+",
                            ((DelayedCounterModel)gameData.Cash.Counters[3]).UnlockValue, gameData.Cash.Counters[0].Name);
                    }
                    foreach (var fundsDriver in gameData.Items)
                    {
                        if (!IsFundsDriverAvailableForBuy(gameData.Cash.Counters[0], fundsDriver))
                        {
                            PrettyConsole.WriteColor(ConsoleColor.Gray,
                                "{3,2}: {0,26} {1,-30}", fundsDriver.Name, "Need to buy: " + fundsDriver.UnlockBalance,
                                 fundsDriver.Bought, ((char)charIndex).ToString());
                        }
                        else
                        {
                            PrettyConsole.WriteColor(ConsoleColor.White, "{3,2}: {0,26} {1,24} x{2,-4} ", fundsDriver.Name,
                                FormatMoney(fundsDriver.Price), fundsDriver.Bought, ((char)charIndex).ToString());
                        }
                        PrettyConsole.WriteColor(ConsoleColor.Red, "+{0, -4} ", GetIncrementorValueById(fundsDriver, 0));
                        PrettyConsole.WriteColor(ConsoleColor.Cyan, "+{0, -7} ", GetIncrementorValueById(fundsDriver, 1));
                        PrettyConsole.WriteColor(ConsoleColor.Green, "+{0,-7} ", GetIncrementorValueById(fundsDriver, 2));
                        Console.WriteLine();
                        charIndex++;
                    }
                }
            }

            private static string FormatMoney(decimal money)
            {
                if (money < 1000)
                {
                    return string.Format("{0:N1}", money);
                }
                var tCount = 1;
                while ((money = money / 1000) > 1000 && tCount < 5)
                {
                    tCount++;
                }

                string suffix = string.Empty;
                switch (tCount)
                {
                    case 1:
                        suffix = "k";
                        break;
                    case 2:
                        suffix = "kk";
                        break;
                    case 3:
                        suffix = "kkk";
                        break;
                    case 4:
                        suffix = "kkkk";
                        break;
                    default:
                        break;
                }
                return string.Format("{0:N1}{1}", money, suffix);
            }
        }

        internal class GameRunner : GameRunnerBase
        {
            private IGameApiController _game;

            private ManualStepResultModel _manualStepResult;
            private int? _verificationResult;


            public GameRunner(string user, PasswordOAuthContext serverContext, IGameApiController game)
            {
                ServerContext = serverContext;
                User = user;
                _game = game;
            }

            public PasswordOAuthContext ServerContext { get; set; }

            public string User { get; set; }

            public ErrorCodes? ErrorCode { get; private set; }

            public void SetGameClient(IGameApiController game)
            {
                _game = game;
            }

            public void ClearGameSession()
            {
                User = null;
                ServerContext = null;
                ErrorCode = null;
            }

            public void PerformManualStep()
            {
                DoActionAndDisplayGameData(() =>
                {
                    _manualStepResult = _game.PerformManualStepAsync(_verificationResult ?? 0).Result;


                    if (_manualStepResult != null && _manualStepResult.VerificationData != null)
                    {
                        Thread.Sleep(100);

                        Console.Clear();
                        Console.WriteLine("Проверка: {0} + {1} = "
                            , _manualStepResult.VerificationData.FirstNumber, _manualStepResult.VerificationData.SecondNumber);
                        int parseResult;
                        var readString = Console.ReadLine();
                        int.TryParse(readString, out parseResult);
                        _verificationResult = parseResult == 0 ? default(int?) : parseResult;
                    }
                });
            }

            public void BuyFundDriver(ConsoleKeyInfo keyInfo)
            {
                DoActionAndDisplayGameData(() => { var res = _game.BuyFundDriverAsync((int)keyInfo.Key - 65).Result; });
            }

            public void FightAgainstCorruption()
            {
                DoActionAndDisplayGameData(() => _game.FightAgainstInflationAsync());
            }

            public override void DisplayGameData(GameDataModel gameData)
            {
                lock (_stdLock)
                {
                    Console.SetCursorPosition(0, 0);
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkRed, "Login: {0}, Server API: {1}", User, ServerContext.BaseUri);
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F2 - Sign out");
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F3 - Rating");
                    Console.SetCursorPosition(0, 3);
                    base.DisplayGameData(gameData);
                }
            }

            public override GameDataModel GetGameData()
            {
                return _game.GetGameDataAsync().Result;
            }

            public void PerformFiveYearPlan()
            {
                DoActionAndDisplayGameData(async () => await _game.ActivateDelayedCounterAsync(3));
            }

            public void Invalidate()
            {
                DoActionAndDisplayGameData(() => {});
            }

            private void DoActionAndDisplayGameData(Action gameAction)
            {
                try
                {
                    gameAction();
                    DisplayGameData(GetGameData());
                }
                catch (AggregateException exp)
                {
                    ErrorCode = ApiHelper.HandleClientException(exp.InnerException as IClientException<ErrorData>);
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exp);
                }

            }

        }
    }
}
