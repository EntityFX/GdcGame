using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Funds;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public interface IGame
    {
        IDictionary<int, FundsDriver> FundsDrivers { get; }

        IDictionary<int, FundsDriver> ModifiedFundsDrivers { get; }

        FundsCounters FundsCounters { get; }

        IDictionary<int, ICustomRule> CustomRules { get; }

        int AutomaticStepNumber { get; }

        int ManualStepNumber { get; }

        void Initialize();

        Task<int> PerformAutoStep(int iterations = 1);

        ManualStepResult PerformManualStep(VerificationManualStepData verificationData);

        BuyFundDriverResult BuyFundDriver(int fundDriverId);

        void FightAgainstInflation();

        void ActivateDelayedCounter(int counterId);

        LotteryResult PlayLottery();

        void Reset();
    }
}
