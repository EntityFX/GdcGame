using System;
using System.Linq;
using System.Threading;
using System.Timers;
using EntityFx.EconomicsArcade.Test.Shared;
using EntityFx.EconomicsArcade.TestApplication.UssrSimulator;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using Timer = System.Timers.Timer;

namespace EntityFx.EconomicsArcade.TestApplication
{
    class GameRunner : GameRunnerBase
    {
        private readonly IMapper<IGame, GameData> _gameDataMapper;
        private readonly IMapper<FundsDriver, EntityFX.EconomicsArcade.Contract.Common.Funds.FundsDriver> _fundsDriverMapper;
        private readonly IGame _game = new UssrSimulatorGame();

        private readonly Timer _timer = new Timer(1000);

        private int? _verificationResult;

        public GameRunner(IMapper<IGame, GameData> gameDataMapper ,IMapper<FundsDriver, EntityFX.EconomicsArcade.Contract.Common.Funds.FundsDriver> fundsDriverMapper)
        {
            _gameDataMapper = gameDataMapper;
            _fundsDriverMapper = fundsDriverMapper;
            _timer.Elapsed += _timer_Elapsed;
        }

        async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await _game.PerformAutoStep();
            DisplayGameData(GetGameData());
        }

        public void Run()
        {
            _game.Initialize();
            _timer.Start();
            DisplayGameData(GetGameData());
        }

        private ManualStepResult _manualStepResult;

        public override GameData GetGameData()
        {
            var gameData = _gameDataMapper.Map(_game);
            gameData.FundsDrivers = _game.FundsDrivers.Select(_ => _fundsDriverMapper.Map(_.Value)).ToArray();
            return gameData;
        }

        public void PerformManualStep()
        {

            _manualStepResult = _game.PerformManualStep(_manualStepResult == null ? null : new VerificationManualStepData() { ResultNumber = _verificationResult ?? 0 });

            if (_manualStepResult is ManualStepNoVerficationRequiredResult)
            {
                _manualStepResult = null;
            }

            if (_manualStepResult != null && _manualStepResult.IsVerificationRequired)
            {
                _timer.Stop();
                Thread.Sleep(100);
                Console.Clear();
                var manualVerificationResult = _manualStepResult as ManualStepVerificationRequiredResult;
                if (manualVerificationResult == null) return;
                Console.WriteLine("Verification required: {0} + {1} = "
                    , manualVerificationResult.FirstNumber, manualVerificationResult.SecondNumber);
                int parseResult;
                var readString = Console.ReadLine();
                int.TryParse(readString, out parseResult);
                _verificationResult = parseResult == 0 ? default(int?) : parseResult;
                _timer.Start();
            }
            DisplayGameData(GetGameData());
        }

        public void BuyFundDriver(ConsoleKeyInfo keyInfo)
        {
            _game.BuyFundDriver((int)keyInfo.Key - 64);
            DisplayGameData(GetGameData());
        }

        public void FightAgainstCorruption()
        {
            _game.FightAgainstInflation();
            DisplayGameData(GetGameData());
        }

        public void PerformFiveYearPlan()
        {
            _game.ActivateDelayedCounter(3);
            DisplayGameData(GetGameData());
        }
    }
}