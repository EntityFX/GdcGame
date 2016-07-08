using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public interface IGame
    {
        IDictionary<int, FundsDriver> FundsDrivers { get; }

        FundsCounters FundsCounters { get; }

        int AutomaticStepNumber { get; }

        int ManualStepNumber { get; }

        void Initialize();

        Task<int> PerformAutoStep();

        ManualStepResult PerformManualStep(VerificationManualStepData verificationData);

        BuyFundDriverResult BuyFundDriver(int fundDriverId);

        void FightAgainstInflation();

        void ActivateDelayedCounter(int counterId);

        LotteryResult PlayLottery();
    }
}
