using System.ServiceModel;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;

    [ServiceContract]
    public interface IGameManager
    {
        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        BuyFundDriverResult BuyFundDriver(int fundDriverId);

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult);

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        void FightAgainstInflation();

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        void PlayLottery();

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        Cash GetCounters();

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        GameData GetGameData();

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        void ActivateDelayedCounter(int counterId);

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        bool Ping();
    }
}