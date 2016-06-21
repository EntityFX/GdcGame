using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager
{
    [ServiceContract]
    public interface IGameManager
    {
        [OperationContract]
        void BuyFundDriver(int fundDriverId);
        [OperationContract]
        void PerformManualStep();
        [OperationContract]
        void FightAgainstInflation();
        [OperationContract]
        void PlayLottery();
        [OperationContract]
        FundsCounters GetCounters();
        [OperationContract]
        GameData GetGameData();
    }
}
