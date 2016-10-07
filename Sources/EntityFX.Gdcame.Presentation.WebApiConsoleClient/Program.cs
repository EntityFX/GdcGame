using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
using Microsoft.Practices.Unity;
using System.Configuration;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    class Program
    {

        private static PasswordOAuthContext _session;

        private static string _userName;
        private static string _userPassword;
        private static UnityContainer _container;

        private static bool _exitFlag;

        private static Dictionary<ConsoleKey, MenuItem> _mainMenu = new Dictionary<ConsoleKey, MenuItem>
        {
            {
                ConsoleKey.F1,
                new MenuItem {MenuText = "Войти и играть", MenuAction = TryLogin}
            },            {
                ConsoleKey.F2,
                new MenuItem {MenuText = "Зарегать аккаунт", MenuAction = RegisterAccount}
            }, {
                ConsoleKey.Escape,
                new MenuItem {MenuText = "Выход", MenuAction = ExitMainMenu}
            }
        };

        private static void ExitMainMenu()
        {
            _exitFlag = true;
        }

        private static void Main(string[] args)
        {
            var listArgs = args.ToList();
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    listArgs.Remove(arg);
                }
            }
            _container = new UnityContainer();

            MainLoop(listArgs);

        }

        private static async Task<Tuple<PasswordOAuthContext, string>> UserLogin(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                Console.Write("Введите логин: ");
                userName = Console.ReadLine();

                Console.Write("Введите пароль: ");
                password = Console.ReadLine();
            }
            var p = new PasswordAuthProvider(new Uri(ConfigurationManager.AppSettings["ServiceBaseAddress"]));
            var res = await p.Login(new PasswordAuthRequest<PasswordAuthData>()
            {
                RequestData = new PasswordAuthData() { Password = password, Usename = userName }
            });
            return new Tuple<PasswordOAuthContext, string>(res, userName);
        }

        private static void RegisterAccount()
        {
            Console.Clear();
            Console.WriteLine("-=Регистрация аккаунта=-");
            Console.Write("Введите логин: ");
            var userName = Console.ReadLine();

            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();

            Console.Write("Подтвердите пароль: ");
            var confirmPassword = Console.ReadLine();

            var authApi = new AuthApiClient(new PasswordOAuthContext() {BaseUri = new Uri(ConfigurationManager.AppSettings["ServiceBaseAddress"]) });

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
                Console.Clear();
                var authException = loginException.InnerException as ClientException;
                if (authException != null)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Ошибка: {0}", authException.Message);
                }
            }


        }

        private async static void UserLogout(PasswordOAuthContext session)
        {
            var authApi = new AuthApiClient(session);
            Console.Clear();
            await authApi.Logout();
            _userName = null;
            _userPassword = null;
            _session = null;
        }

        private static IGameApiController GetGameClient(PasswordOAuthContext session)
        {
            return new GameApiClient(session);
        }

        private static IAdminController GetAdminClient(PasswordOAuthContext session)
        {
            return new AdminApiClient(session);
        }

        private static void TryLogin()
        {
            Tuple<PasswordOAuthContext, string> loginResultTuple;
            try
            {
                Console.Clear();
                Console.WriteLine("-=Вход=-");
                loginResultTuple = UserLogin(_userName, _userPassword).Result;
            }
            catch (AggregateException loginException)
            {
                Console.Clear();
                var authException = loginException.InnerException as WrongAuthException<PasswordAuthData>;
                if (authException != null)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Ошибка: {0}", authException.Message);
                }
                loginResultTuple = null;
            }

            if (loginResultTuple != null)
            {
                _session = loginResultTuple.Item1;
                _userName = loginResultTuple.Item2;
                EnterGame();
            }
        }

        private static void EnterMainMenu()
        {
            _exitFlag = false;
            while (!_exitFlag)
            {
                Console.WriteLine("-=Администрирование=-");
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

        private static void EnterGame()
        {
            Console.Clear();
            var gameClient = GetGameClient(_session);
            var gr = new GameRunner(_userName, _session, gameClient);
            var adminManagerClient = GetAdminClient(_session);
            var ac = new AdminConsole( adminManagerClient);
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
                        UserLogout(_session);
                        break;
                    }
                }
                catch (ClientException faultException)
                {
                    var res = UserLogin(null, null).Result;
                    _session = res.Item1;
                    _userName = res.Item2;
                    gr.SessionGuid = _session;
                    //ac.SessionGuid = _session;
                    gr.User = _userName;
                    gr.SetGameClient(GetGameClient(_session));
                    //ac.SetAdminClient(GetAdminClient(_sessionGuid));
                    gr.DisplayGameData(gr.GetGameData());
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Ошибка: {0}", faultException);
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
                    Console.WriteLine("Баланс: {0:C}; Всего заработано: {1:C}", gameData.Cash.OnHand,
                        gameData.Cash.TotalEarned);
                    /*Console.WriteLine("Manual Steps: {0}, Automatic Steps: {1}",
                        gameData.ManualStepsCount, gameData.AutomatedStepsCount);*/
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "{1,15}: {0,12}", gameData.Cash.Counters[0].Value,
                        gameData.Cash.Counters[0].Name);
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ",
                        ((GenericCounterModel)gameData.Cash.Counters[1]).SubValue, gameData.Cash.Counters[1].Name);
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C} ({2}%)"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Bonus, "Бонус"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).BonusPercentage);
                    PrettyConsole.WriteColor(ConsoleColor.Cyan, "{1,15}: {0,12}%"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Inflation, "Батхёрт");
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Value, "Total");
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}",
                        ((GenericCounterModel)gameData.Cash.Counters[2]).SubValue, gameData.Cash.Counters[2].Name);
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Bonus, "Бонус"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).BonusPercentage);
                    PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Inflation, "Усталость");
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Value, "Всего");
                    PrettyConsole.WriteLineColor(ConsoleColor.Magenta, "{1,15}: +{0,12:C} {2}/{3}",
                        gameData.Cash.Counters[3].Value, gameData.Cash.Counters[3].Name
                        , TimeSpan.FromSeconds(((DelayedCounterModel)gameData.Cash.Counters[3]).SecondsRemaining)
                        , 0);

                    Console.WriteLine();
                    var charIndex = 65;
                    PrettyConsole.WriteLineColor(
                        IsCounterWithInflation((GenericCounterModel)gameData.Cash.Counters[2])
                            ? ConsoleColor.Yellow
                            : ConsoleColor.DarkYellow, "{0,2}:   Отдохнуть", "*");
                    if (IsCounterAvailableForActivate((DelayedCounterModel)gameData.Cash.Counters[3],
                        gameData.Cash.Counters[0].Value))
                    {
                        PrettyConsole.WriteLineColor(
                            IsCounterIsMining((DelayedCounterModel)gameData.Cash.Counters[3])
                                ? ConsoleColor.DarkMagenta
                                : ConsoleColor.Magenta, "{0,2}:         Делать кварталку", "+");
                    }
                    else
                    {
                        PrettyConsole.WriteLineColor(ConsoleColor.DarkMagenta,
                            "{0,2}:         Делать кварталку: надо {1} {2} чтоб разлочить", "+",
                            ((DelayedCounterModel)gameData.Cash.Counters[3]).UnlockValue, gameData.Cash.Counters[0].Name);
                    }
                    foreach (var fundsDriver in gameData.Items)
                    {
                        if (!IsFundsDriverAvailableForBuy(gameData.Cash.Counters[0], fundsDriver))
                        {
                            PrettyConsole.WriteColor(ConsoleColor.Gray,
                                "{0,2}:           Надо для покупки:     {1,6} x{2,-4} ", ((char)charIndex).ToString(),
                                fundsDriver.UnlockBalance, fundsDriver.Bought);
                        }
                        else
                        {
                            PrettyConsole.WriteColor(ConsoleColor.White, "{3,2}: {0,26} {1,14:C} x{2,-4} ", fundsDriver.Name,
                                fundsDriver.Price, fundsDriver.Bought, ((char)charIndex).ToString());
                        }
                        PrettyConsole.WriteColor(ConsoleColor.Red, "+{0, -4} ", GetIncrementorValueById(fundsDriver, 0));
                        PrettyConsole.WriteColor(ConsoleColor.Cyan, "+{0, -7} ", GetIncrementorValueById(fundsDriver, 1));
                        PrettyConsole.WriteColor(ConsoleColor.Green, "+{0,-7} ", GetIncrementorValueById(fundsDriver, 2));
                        Console.WriteLine();
                        charIndex++;
                    }
                }
            }
        }

        internal class GameRunner : GameRunnerBase
        {
            private IGameApiController _game;

            private ManualStepResultModel _manualStepResult;
            private int? _verificationResult;

            public GameRunner(string user, PasswordOAuthContext session, IGameApiController game)
            {
                SessionGuid = session;
                User = user;
                _game = game;
            }

            public PasswordOAuthContext SessionGuid { get; set; }

            public string User { get; set; }

            public void SetGameClient(IGameApiController game)
            {
                _game = game;
            }

            public async void PerformManualStep()
            {
                try
                {
                    _manualStepResult =
                        await
                                    _game.PerformManualStepAsync(_verificationResult ?? 0);


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
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Ошибка: {0}", exp);
                }
                DisplayGameData(GetGameData());
            }

            public async void BuyFundDriver(ConsoleKeyInfo keyInfo)
            {
                try
                {
                    await _game.BuyFundDriverAsync((int)keyInfo.Key - 64);
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Ошибка: {0}", exp);
                }
                DisplayGameData(GetGameData());
            }

            public async void FightAgainstCorruption()
            {
                try
                {
                    await _game.FightAgainstInflationAsync();
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Ошибка: {0}", exp);
                }
                DisplayGameData(GetGameData());
            }

            public override void DisplayGameData(GameDataModel gameData)
            {
                lock (_stdLock)
                {
                    Console.SetCursorPosition(0, 0);
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkRed, "Логин: {0}", User);
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F2 - Администрирование");
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F3 - Разлогиниться");
                    Console.SetCursorPosition(0, 3);
                    base.DisplayGameData(gameData);
                }
            }

            public override GameDataModel GetGameData()
            {
                return _game.GetGameDataAsync().Result;
            }

            public async void PerformFiveYearPlan()
            {
                await _game.ActivateDelayedCounterAsync(3);
                DisplayGameData(GetGameData());
            }

            public void Invalidate()
            {
                DisplayGameData(GetGameData());
            }
        }
    }
}
