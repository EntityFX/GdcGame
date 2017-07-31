namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData
{
    using System.ServiceModel;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Contract.MainServer;

    [ServiceContract]
    public interface IGameDataRetrieveDataAccessService
    {
        [OperationContract]
        GameData GetGameData(string userId);

        [OperationContract]
        RatingStatistics[] GetUserRatings();
    }
}