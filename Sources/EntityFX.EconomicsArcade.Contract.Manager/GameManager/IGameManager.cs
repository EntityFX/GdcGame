using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager
{
    [ServiceContract]
    public interface IGameManager
    {
        [OperationContract]
        BuyFundDriverResult BuyFundDriver(int fundDriverId);
        [OperationContract]
        ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult);
        [OperationContract]
        void FightAgainstInflation();
        [OperationContract]
        void PlayLottery();
        [OperationContract]
        FundsCounters GetCounters();
        [OperationContract]
        GameData GetGameData();
        [OperationContract]
        void ActivateDelayedCounter(int counterId);
    }
}
