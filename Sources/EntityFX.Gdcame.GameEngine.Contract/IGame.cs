using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Items;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public interface IGame
    {
        ReadOnlyDictionary<int, Item> Items { get; }

        Dictionary<int, Item> ModifiedFundsDrivers { get; }

        GameCash GameCash { get; }

        ReadOnlyDictionary<int, ICustomRule> CustomRules { get; }

        int AutomaticStepNumber { get; }

        int ManualStepNumber { get; }

        void Initialize();

        void PerformAutoStep(int iterations = 1);

        ManualStepResult PerformManualStep(VerificationManualStepData verificationData);

        BuyItemResult BuyFundDriver(int fundDriverId);

        void FightAgainstInflation();

        void ActivateDelayedCounter(int counterId);

        LotteryResult PlayLottery();

        void Reset();
    }
}