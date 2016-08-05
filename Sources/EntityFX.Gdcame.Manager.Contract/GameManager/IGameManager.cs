using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;

namespace EntityFX.Gdcame.Manager.Contract.GameManager
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
