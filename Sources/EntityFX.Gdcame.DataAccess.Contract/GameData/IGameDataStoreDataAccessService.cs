using System.ServiceModel;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [ServiceContract]
    public interface IGameDataStoreDataAccessService
    {
        [OperationContract(IsOneWay = true)]
        void StoreGameDataForUsers(StoredGameDataWithUserId[] listOfGameDataWithUserId);
    }
}