using System;
using EntityFx.EconomicsArcade.Test.Shared;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.TestClient
{
    public class GameRunner : GameRunnerBase
    {
        private readonly Guid _sessionGuid;
        private readonly string _user;
        private readonly IGameManager _game;
        private ILogger _logger;

        public GameRunner(ILogger logger, string user, Guid sessionGuid, IGameManager game)
        {
            _logger = logger;
            _sessionGuid = sessionGuid;
            _user = user;
            _game = game;
        }

        public void PerformManualStep()
        {
            _logger.Trace("EntityFX.EconomicsArcade.TestClient.GameRunner.PerformManualStep():");

            try
            {
                var result = _game.PerformManualStep(null);
                DisplayGameData(GetGameData());
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
            }
        }

        public void BuyFundDriver(ConsoleKeyInfo keyInfo)
        {
            _logger.Trace("EntityFX.EconomicsArcade.TestClient.GameRunner.BuyFundDriver():");
            _logger.Info("keyInfo.Key is {0}", keyInfo.Key);

            try
            {
                _game.BuyFundDriver((int)keyInfo.Key - 64);
                DisplayGameData(GetGameData());
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
            }
        }

        public void FightAgainstCorruption()
        {
            try
            {
                _game.FightAgainstInflation();
                DisplayGameData(GetGameData());
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        
        public override void DisplayGameData(GameData gameData)
        {
            Console.SetCursorPosition(0, 0);
            PrettyConsole.WriteLineColor(ConsoleColor.DarkRed, "User: {0}, Session: {1}", _user, _sessionGuid);
            Console.SetCursorPosition(0, 1);
            base.DisplayGameData(gameData);
        }

        public override  GameData GetGameData()
        {
            return _game.GetGameData();
        }

        public void PerformFiveYearPlan()
        {
            _game.ActivateDelayedCounter(3);
            DisplayGameData(GetGameData());
        }

    }
}