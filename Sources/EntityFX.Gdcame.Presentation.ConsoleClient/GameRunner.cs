﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Presentation.ConsoleClient
{
    public class GameRunner : GameRunnerBase
    {
        private IGameManager _game;

        private ManualStepResult _manualStepResult;
        private int? _verificationResult;

        public GameRunner(string user, Guid sessionGuid, IGameManager game)
        {
            SessionGuid = sessionGuid;
            User = user;
            _game = game;
        }

        public Guid SessionGuid { get; set; }

        public string User { get; set; }

        public void SetGameClient(IGameManager game)
        {
            _game = game;
        }

        public async void PerformManualStep()
        {
            try
            {
                _manualStepResult =
                    await
                        Task.Run(
                            () =>
                                _game.PerformManualStep(_manualStepResult == null
                                    ? null
                                    : new VerificationManualStepResult {VerificationNumber = _verificationResult ?? 0}));

                if (_manualStepResult is NoVerficationRequiredResult)
                {
                    _manualStepResult = null;
                }

                if (_manualStepResult is VerificationRequiredResult)
                {
                    Thread.Sleep(100);
                    Console.Clear();
                    var manualVerificationResult = _manualStepResult as VerificationRequiredResult;
                    Console.WriteLine("Verification required: {0} + {1} = "
                        , manualVerificationResult.FirstNumber, manualVerificationResult.SecondNumber);
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

        public void BuyFundDriver(ConsoleKeyInfo keyInfo)
        {
            try
            {
                _game.BuyFundDriver((int) keyInfo.Key - 64);
            }
            catch (Exception exp)
            {
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exp);
            }
            DisplayGameData(GetGameData());
        }

        public void FightAgainstCorruption()
        {
            try
            {
                _game.FightAgainstInflation();
            }
            catch (Exception exp)
            {
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exp);
            }
            DisplayGameData(GetGameData());
        }

        public override void DisplayGameData(GameData gameData)
        {
            lock (_stdLock)
            {
                Console.SetCursorPosition(0, 0);
                PrettyConsole.WriteLineColor(ConsoleColor.DarkRed, "User: {0}, Session: {1}", User, SessionGuid);
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F2 - Admin settings");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F3 - Logout");
                Console.SetCursorPosition(0, 3);
                base.DisplayGameData(gameData);
            }
        }

        public override GameData GetGameData()
        {
            return _game.GetGameData();
        }

        public void PerformFiveYearPlan()
        {
            _game.ActivateDelayedCounter(3);
            DisplayGameData(GetGameData());
        }

        public void Invalidate()
        {
            DisplayGameData(GetGameData());
        }
    }
}