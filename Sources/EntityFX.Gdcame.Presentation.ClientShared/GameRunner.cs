using System;
using System.Threading;
using EntityFx.GdCame.Presentation.Shared;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Presentation.ConsoleClient.Common;
using EntityFX.Presentation.Shared.GameConsole;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Presentation.ClientShared
{

        public class GameRunner : GameRunnerBase
        {
            private readonly ApiHelper<IAuthenticator> _apiHelper;
            private IGameApiController _game;

            private ManualStepResultModel _manualStepResult;
            private int? _verificationResult;


            public GameRunner(ApiHelper<IAuthenticator> apiHelper, string user, IAuthContext<IAuthenticator> serverContext, IGameApiController game)
            {
                ServerContext = serverContext;
                User = user;
                _apiHelper = apiHelper;
                _game = game;
            }

            public IAuthContext<IAuthenticator> ServerContext { get; set; }

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
                    ErrorCode = _apiHelper.HandleClientException(exp.InnerException as IClientException<ErrorData>);
                }
                catch (Exception exp)
                {
                    PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exp);
                }

            }

        }
}
