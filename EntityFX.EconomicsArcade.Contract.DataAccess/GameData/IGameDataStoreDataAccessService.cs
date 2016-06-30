using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Contract.DataAccess.GameData
{
    [ServiceContract]
    public interface IGameDataStoreDataAccessService
    {
        [OperationContract(IsOneWay = true)]
        void StoreGameDataForUser(int userId, Common.GameData gameData);
    }
}