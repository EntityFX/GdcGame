using EntityFx.EconomicsArcade.TestApplication.UssrSimulator;
using EntityFX.EconomicsArcade.Contract.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EntityFx.EconomicsArcade.TestApplication
{
    class Program
    {
        private static readonly IGame _game = new UssrSimulatorGame();
        
        static void Main(string[] args)
        {
            _game.Initialize();
            var timer = new Timer(1000);
            timer.Elapsed += timer_Elapsed;
            /*g.BuyFundDriver(1);
            g.BuyFundDriver(2);
            g.BuyFundDriver(9);
            g.BuyFundDriver(19); */
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    _game.PerformManualStep();
                }
                else if ((int)keyInfo.Key >= 65 && (int)keyInfo.Key <= 90)
                {
                    _game.BuyFundDriver((int)keyInfo.Key - 64);
                }
            }
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
