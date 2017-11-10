//

namespace EntityFX.Gdcame.Manager.Contract.MainServer.GameManager
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;

    //
    public interface IGameManager
    {
        //
        //[FaultContract(typeof (InvalidSessionFault))]
        BuyFundDriverResult BuyFundDriver(int fundDriverId);

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult);

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        void FightAgainstInflation();

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        void PlayLottery();

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        Cash GetCounters();

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        GameData GetGameData();

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        void ActivateDelayedCounter(int counterId);

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        bool Ping();
    }
}