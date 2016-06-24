using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Contract.DataAccess.GameData
{
    [ServiceContract]
    public interface IGameDataRetrieveDataAccessService
    {
        [OperationContract]
        Common.GameData GetGameData(int userId);
    }
}
