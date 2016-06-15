using EntityFx.EconomicsArcade.TestApplication.UssrSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.EconomicsArcade.TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new UssrSimulatorGame();
            g.Initialize();
            g.Start();
            /*g.BuyFundDriver(1);
            g.BuyFundDriver(2);
            g.BuyFundDriver(9);
            g.BuyFundDriver(19); */
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    g.PerformManualStep();
                }
                else if ((int)keyInfo.Key >= 65 && (int)keyInfo.Key <= 90)
                {
                    g.BuyFundDriver((int)keyInfo.Key - 64);
                }
            }
        }
    }
}
