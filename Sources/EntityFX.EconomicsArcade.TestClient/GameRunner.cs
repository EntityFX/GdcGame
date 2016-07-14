using System;
using System.Threading.Tasks;
using EntityFx.EconomicsArcade.Test.Shared;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;

namespace EntityFX.EconomicsArcade.TestClient
{
    public class GameRunner : GameRunnerBase
    {
        private Guid _sessionGuid;
        private string _user;
        private IGameManager _game;

        public GameRunner(string user, Guid sessionGuid, IGameManager game)
        {
            _sessionGuid = sessionGuid;
            _user = user;
            _game = game;
        }

        public Guid SessionGuid
        {
            get { return _sessionGuid; }
            set { _sessionGuid = value; }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        public void SetGameClient(IGameManager game)
        {
            _game = game;
        }

        public async void PerformManualStep()
        {
            try
            {
                await Task.Run(() => _game.PerformManualStep(null));
            }
            catch (Exception exp)
            {

            }
            DisplayGameData(GetGameData());
        }

        public void BuyFundDriver(ConsoleKeyInfo keyInfo)
        {

            try
            {
                _game.BuyFundDriver((int)keyInfo.Key - 64);
            }
            catch (Exception exp)
            {

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
                Console.WriteLine(exp);
            }
            DisplayGameData(GetGameData());
        }

        private static readonly object _stdLock = new {};

        public override void DisplayGameData(GameData gameData)
        {
            lock (_stdLock)
            {
                Console.SetCursorPosition(0, 0);
                PrettyConsole.WriteLineColor(ConsoleColor.DarkRed, "User: {0}, Session: {1}", _user, _sessionGuid);
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F2 - Admin settings");
                PrettyConsole.WriteLineColor(ConsoleColor.DarkGreen, "F3 - Logout");
                Console.SetCursorPosition(0, 3);
                base.DisplayGameData(gameData);
            }
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

        public void Invalidate()
        {
            DisplayGameData(GetGameData());
        }

    }
}