using System;
using EntityFX.Presentation.Shared.KernelConsole;

namespace EntityFX.Gdcame.EngineTestApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainLoop();
        }

        private static void MainLoop()
        {
            var gr = new GameRunner(new GameDataMapper(), new Presentation.Shared.KernelConsole.FundsDriverContractMapper());
            gr.Run();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)

            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    gr.PerformManualStep();
                }
                else if ((int) keyInfo.Key >= 65 && (int) keyInfo.Key <= 90)
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
            }
        }
    }
}