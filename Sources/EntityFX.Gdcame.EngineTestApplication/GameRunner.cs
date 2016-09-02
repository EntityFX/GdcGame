using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.EngineTestApplication.UssrSimulator;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;
using Timer = System.Timers.Timer;

namespace EntityFX.Gdcame.EngineTestApplication
{
    internal class GameRunner : GameRunnerBase
    {
        private readonly IMapper<Item, EntityFX.Gdcame.Common.Contract.Items.Item> _fundsDriverMapper;
        private readonly IGame _game = new UssrSimulatorGame();
        private readonly IMapper<IGame, GameData> _gameDataMapper;

        private readonly Timer _timer = new Timer(1000);

        private ManualStepResult _manualStepResult;

        private int? _verificationResult;

        public GameRunner(IMapper<IGame, GameData> gameDataMapper,
            IMapper<Item, EntityFX.Gdcame.Common.Contract.Items.Item> fundsDriverMapper)
        {
            _gameDataMapper = gameDataMapper;
            _fundsDriverMapper = fundsDriverMapper;
            _timer.Elapsed += _timer_Elapsed;
        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            await _game.PerformAutoStep();
            Console.WriteLine(sw.Elapsed);
            DisplayGameData(GetGameData());
        }

        public void Run()
        {
            _game.Initialize();
            _timer.Start();
            DisplayGameData(GetGameData());
        }

        public override void DisplayGameData(GameData gameData)
        {
            lock (_stdLock)
            {
#if __MonoCS__
				Console.Clear();
				#else

#endif
                Console.SetCursorPosition(0, 0);
                base.DisplayGameData(gameData);
            }
        }

        public override GameData GetGameData()
        {
            var gameData = _gameDataMapper.Map(_game);
            gameData.Items = _game.Items.Select(_ => _fundsDriverMapper.Map(_.Value)).ToArray();
            return gameData;
        }

        public void PerformManualStep()
        {
            _manualStepResult =
                _game.PerformManualStep(_manualStepResult == null
                    ? null
                    : new VerificationManualStepData {ResultNumber = _verificationResult ?? 0});

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
            _game.BuyFundDriver((int) keyInfo.Key - 64);
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