namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData
{
    using System.ServiceModel;
    using EntityFX.Gdcame.Contract.MainServer.Store;


    [ServiceContract]
    public interface IGameDataStoreDataAccessService
    {
        [OperationContract(IsOneWay = true)]
        void StoreGameDataForUsers(StoredGameDataWithUserId[] listOfGameDataWithUserId);
    }
}