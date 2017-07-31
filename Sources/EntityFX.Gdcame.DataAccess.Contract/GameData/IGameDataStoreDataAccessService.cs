namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData
{
    using System.ServiceModel;

    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;

    [ServiceContract]
    public interface IGameDataStoreDataAccessService
    {
        [OperationContract(IsOneWay = true)]
        void StoreGameDataForUsers(StoredGameDataWithUserId[] listOfGameDataWithUserId);
    }
}