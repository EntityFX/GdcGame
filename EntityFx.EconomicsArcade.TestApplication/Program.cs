using EntityFx.EconomicsArcade.TestApplication.UssrSimulator;
using EntityFX.EconomicsArcade.Contract.Game;
using System;
using System.Timers;

namespace EntityFx.EconomicsArcade.TestApplication
{
    class Program
    {


        static void Main(string[] args)
        {
            MainLoop();
        }

        static void MainLoop()
        {
            var gr = new GameRunner();
            gr.Run();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            //while ((keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Enter, false, false, false)).Key != ConsoleKey.Escape)
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
            }
        }

    }

    class GameRunner
    {
        private readonly IGame _game = new UssrSimulatorGame();

        private readonly Timer _timer = new Timer(1000);

        public GameRunner()
        {
            _timer.Elapsed += _timer_Elapsed;
        }

        async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await _game.PerformAutoStep();
        }

        public void Run()
        {
            _game.Initialize();
            _timer.Start();
        }

        public void PerformManualStep()
        {
            _game.PerformManualStep();
        }

        public void BuyFundDriver(ConsoleKeyInfo keyInfo)
        {
            _game.BuyFundDriver((int)keyInfo.Key - 64);
        }

        public void FightAgainstCorruption()
        {
            _game.FightAgainstInflation();
        }
    }
}
