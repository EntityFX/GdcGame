using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Presentation.Contract.Controller;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    class Program
    {

        private static PasswordOAuthContext _session;

        private static string _userName;
        private static string _userPassword;
        private static UnityContainer _container;

        private static void Main(string[] args)
        {
            var listArgs = args.ToList();
            var isCollapsed = false;
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    isCollapsed = arg.Contains("IsCollapsed");
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
                Console.Write("Please, enter user name: ");
                userName = Console.ReadLine();

                Console.Write("Please, enter user password: ");
                password = Console.ReadLine();
            }
            var p = new PasswordAuthProvider(new Uri("http://localhost:9001"));
            var res = await p.Login(new PasswordAuthRequest<PasswordAuthData>()
            {
                RequestData = new PasswordAuthData() { Password = password, Usename = userName }
            });
            return new Tuple<PasswordOAuthContext, string>(res, userName);
        }

        private static bool UserLogout(PasswordOAuthContext session)
        {
            return false;
        }

        private static IGameApiController GetGameClient(PasswordOAuthContext session)
        {
            return new GameApiClient(session);
        }

        private static IGameApiController GetAdminClient(PasswordOAuthContext session)
        {
            return null;
        }

        private static bool TryLogin(string userName, string password, out Tuple<PasswordOAuthContext, string> loginResultTuple)
        {
            try
            {
                loginResultTuple = UserLogin(_userName, _userPassword).Result;
            }
            catch (AggregateException loginException)
            {
                var authException = loginException.InnerException as WrongAuthException<PasswordAuthData>;
                if (authException != null)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", authException.Message);
                }
                loginResultTuple = null;
                return false;
            }
            return true;
        }

        private static void MainLoop(IEnumerable<string> args)
        {
            if (args.Any())
            {
                _userName = args.First();
                if (args.Count() >= 2)
                {
                    _userPassword = args.ToArray()[1];
                }
            }
            Tuple<PasswordOAuthContext, string> loginResult = null;
            do
            {
            } while ((!TryLogin(_userName, _userPassword, out loginResult)));

            if (loginResult != null)
            {
                _session = loginResult.Item1;
                _userName = loginResult.Item2;
            }


            var adminManagerClient = GetAdminClient(_session);

            var gameClient = GetGameClient(_session);



            var gr = new GameRunner(_userName, _session, gameClient);
            var ac = new AdminConsole(/*adminManagerClient, _sessionGuid*/);
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
                        gr.DisplayGameData(gr.GetGameData());
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
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", faultException);
                }
            }
        }

        internal abstract class GameRunnerBase
        {
            protected static readonly object _stdLock = new { };

            private bool IsFundsDriverAvailableForBuy(CounterModelBase rootCounter, ItemModel item)
            {
                return item.UnlockValue <= rootCounter.Value;
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
                    return string.Format("{0}{1}", incrementor.Value,
                        incrementor.IncrementorType == IncrementorTypeEnum.PercentageIncrementor ? "%" : string.Empty);
                }
                return "0";
            }

            public abstract GameDataModel GetGameData();

            public virtual void DisplayGameData(GameDataModel gameData)
            {
                lock (_stdLock)
                {
                    Console.WriteLine("Funds: {0:C}; Total Funds: {1:C}", gameData.Cash.OnHand,
                        gameData.Cash.Total);
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
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Inflation, "Inflation");
                    PrettyConsole.WriteLineColor(ConsoleColor.Cyan, "{1,15}: {0,12:C}"
                        , ((GenericCounterModel)gameData.Cash.Counters[1]).Value, "Total");
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C}",
                        ((GenericCounterModel)gameData.Cash.Counters[2]).SubValue, gameData.Cash.Counters[2].Name);
                    PrettyConsole.WriteLineColor(ConsoleColor.Green, "{1,15}: {0,12:C} ({2}%)"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Bonus, "Bonus"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).BonusPercentage);
                    PrettyConsole.WriteColor(ConsoleColor.Green, "{1,15}: {0,12}%"
                        , ((GenericCounterModel)gameData.Cash.Counters[2]).Inflation, "Corruption");
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
                            : ConsoleColor.DarkYellow, "{0,2}:   Fight Against Corruption", "*");
                    if (IsCounterAvailableForActivate((DelayedCounterModel)gameData.Cash.Counters[3],
                        gameData.Cash.Counters[0].Value))
                    {
                        PrettyConsole.WriteLineColor(
                            IsCounterIsMining((DelayedCounterModel)gameData.Cash.Counters[3])
                                ? ConsoleColor.DarkMagenta
                                : ConsoleColor.Magenta, "{0,2}:         Do Five Year Plan", "+");
                    }
                    else
                    {
                        PrettyConsole.WriteLineColor(ConsoleColor.DarkMagenta,
                            "{0,2}:         Do Five Year Plan: need {1} {2} to unlock", "+",
                            ((DelayedCounterModel)gameData.Cash.Counters[3]).UnlockValue, gameData.Cash.Counters[0].Name);
                    }
                    foreach (var fundsDriver in gameData.Items)
                    {
                        if (!IsFundsDriverAvailableForBuy(gameData.Cash.Counters[0], fundsDriver))
                        {
                            PrettyConsole.WriteColor(ConsoleColor.Gray,
                                "{0,2}:             Need money to buy:     {1,8}. x{2,-4} ", ((char)charIndex).ToString(),
                                fundsDriver.UnlockValue, fundsDriver.BuyCount);
                        }
                        else
                        {
                            PrettyConsole.WriteColor(ConsoleColor.White, "{3,2}: {0,28} {1,15:C} x{2,-4} ", fundsDriver.Name,
                                fundsDriver.Value, fundsDriver.BuyCount, ((char)charIndex).ToString());
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
                        Console.WriteLine("Verification required: {0} + {1} = "
                            , _manualStepResult.VerificationData.FirstNumber, _manualStepResult.VerificationData.SecondNumber);
                        int parseResult;
                        var readString = Console.ReadLine();
                        int.TryParse(readString, out parseResult);
                        _verificationResult = parseResult == 0 ? default(int?) : parseResult;
                    }
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exp);
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
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exp);
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
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exp);
                }
                DisplayGameData(GetGameData());
            }

            public override void DisplayGameData(GameDataModel gameData)
            {
                lock (_stdLock)
                {
                    Console.SetCursorPosition(0, 0);
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkRed, "User: {0}", User);
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F2 - Admin settings");
                    PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F3 - Logout");
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
