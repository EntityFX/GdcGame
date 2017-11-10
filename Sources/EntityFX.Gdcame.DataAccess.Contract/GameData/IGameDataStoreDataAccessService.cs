namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData
{
    //
    using EntityFX.Gdcame.Contract.MainServer.Store;


    //
    public interface IGameDataStoreDataAccessService
    {
        //[OperationContract(IsOneWay = true)]
        void StoreGameDataForUsers(StoredGameDataWithUserId[] listOfGameDataWithUserId);
    }
}