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
        [FaultContract(typeof(InvalidSessionFault))]
        BuyFundDriverResult BuyFundDriver(int fundDriverId);
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult);
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        void FightAgainstInflation();
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        void PlayLottery();
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        FundsCounters GetCounters();
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        GameData GetGameData();
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        void ActivateDelayedCounter(int counterId);
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        bool Ping();
    }
}
