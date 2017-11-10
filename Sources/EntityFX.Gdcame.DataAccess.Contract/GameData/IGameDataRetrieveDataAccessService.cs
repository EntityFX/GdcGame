namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData
{
    //

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Store;

    //
    public interface IGameDataRetrieveDataAccessService
    {
        //
        GameData GetGameData(string userId);

        //
        StoredGameDataWithUserId[] GetStoredGameData(string[] userIds);

        //
        RatingStatistics[] GetUserRatings();
    }
}