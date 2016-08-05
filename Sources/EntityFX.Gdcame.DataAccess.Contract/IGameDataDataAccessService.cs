using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Contract.DataAccess.GameData
{
    [ServiceContract]
    public interface IGameDataDataAccessService
    {
        [OperationContract]
        Common.GameData GetGameData();
    }
}
