using System.ServiceModel;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [ServiceContract]
    public interface IGameDataStoreDataAccessService
    {
        [OperationContract(IsOneWay = true)]
        void StoreGameDataForUser(int userId, Common.Contract.GameData gameData);
    }
}